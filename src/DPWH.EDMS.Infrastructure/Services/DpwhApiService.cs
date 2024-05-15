using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Helpers;
using DPWH.EDMS.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace DPWH.EDMS.Infrastructure.Services;

public class DpwhApiService : IDpwhApiService
{
    private const string MediaType = "application/soap+xml";

    private readonly HttpClient _httpClient;
    private readonly ILogger<DpwhApiService> _logger;
    private readonly DpwhIntegrationSettings _settings;
    private readonly AsyncRetryPolicy _retryPolicy;

    public DpwhApiService(ILogger<DpwhApiService> logger, IHttpClientFactory httpClientFactory, ConfigManager config)
    {
        _logger = logger;
        _settings = config.DpwhIntegration;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

        _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(3));
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<T?> Get<T>(string operation, string? employeeId = null)
    {
        try
        {
            var response = await _httpClient.SendAsync(CreateRequestMessage(operation, employeeId));

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("DPWH Api return unsuccessful status `{StatusCode}`: {Reason}", response.StatusCode, response.ReasonPhrase);
                throw new ApiException(response.ReasonPhrase, response.StatusCode, errorContent);
            }

            var xmlResult = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Received message {Message}", xmlResult);

            var xmlSerializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xmlResult);
            return (T?)xmlSerializer.Deserialize(reader);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while calling DWPH Soap WebService `{Operation}`", operation);
            throw;
        }
    }

    public async Task<T?> GetWithRetry<T>(string operation, string? employeeId = null)
    {
        return await _retryPolicy.ExecuteAsync(async () => await Get<T>(operation, employeeId));
    }

    public async Task<T?> GetLocation<T>(string operation, string? type = null, string? id = null)
    {
        try
        {
            var request = CreateRequestMessageLocation(operation, type, id);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("DPWH Api return unsuccessful status `{StatusCode}`: {Reason}", response.StatusCode, response.ReasonPhrase);
                throw new ApiException(response.ReasonPhrase, response.StatusCode, errorContent);
            }

            var xmlResult = await response.Content.ReadAsStringAsync();
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xmlResult);

            return (T?)xmlSerializer.Deserialize(reader);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while calling DWPH Soap WebService `{Operation}`", operation);
            throw;
        }
    }
    public async Task<T?> GetLocationWithRetry<T>(string operation, string? type = null, string? id = null)
    {
        return await _retryPolicy.ExecuteAsync(async () => await GetLocation<T>(operation, type, id));
    }

    public async Task<string> GetRaw(string operation, string? employeeId = null)
    {
        try
        {
            var request = CreateRequestMessage(operation, employeeId);
            var response = await _httpClient.SendAsync(request);
            var xmlResult = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Received message {Message}", xmlResult);
            return xmlResult;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while calling DWPH Soap WebService `{Operation}`", operation);
            throw;
        }
    }

    private HttpRequestMessage CreateRequestMessage(string operation, string? employeeId)
    {
        var uri = $"{_settings.BaseApiUrl}?op={operation}";
        _logger.LogInformation("Sending request to {Uri}", uri);

        var httpRequestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(uri),
            Method = HttpMethod.Post
        };

        var requestBody = BuildRequestBody(operation, employeeId).InnerXml;
        _logger.LogInformation("Sending message {Message}", requestBody);

        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaType);
        httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
        httpRequestMessage.Headers.Add("Host", "apps.dpwh.gov.ph");

        return httpRequestMessage;
    }

    private HttpRequestMessage CreateRequestMessageLocation(string operation, string? type, string? id)
    {
        var uri = $"{_settings.BaseApiUrl}?op={operation}";
        _logger.LogInformation("Sending request to {Uri}", uri);

        var httpRequestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(uri),
            Method = HttpMethod.Post
        };

        var requestBody = BuildRequestBodyLocation(operation, type, id).InnerXml;
        _logger.LogInformation("Sending message {Message}", requestBody);

        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaType);
        httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
        httpRequestMessage.Headers.Add("Host", "apps.dpwh.gov.ph");

        return httpRequestMessage;
    }

    private XmlDocument BuildRequestBody(string operation, string? employeeId)
    {
        var xmlDocument = new XmlDocument();
        var requestElement = xmlDocument.CreateElement("", operation, SoapRequestBuilder.DpwhNamespace);
        if (employeeId is not null)
        {
            var employeeIdElement = xmlDocument.CreateElement("", "employee_id", SoapRequestBuilder.DpwhNamespace);
            employeeIdElement.InnerText = employeeId;
            requestElement.AppendChild(employeeIdElement);
        }

        xmlDocument.AppendChild(requestElement);

        return new SoapRequestBuilder(_settings)
            .WithRequest(xmlDocument.DocumentElement)
            .Build();
    }
    private XmlDocument BuildRequestBodyLocation(string operation, string? type, string? id)
    {
        var xmlDocument = new XmlDocument();
        var requestElement = xmlDocument.CreateElement("", operation, SoapRequestBuilder.DpwhNamespace);

        var typeElement = xmlDocument.CreateElement("", "type", SoapRequestBuilder.DpwhNamespace);
        if (!string.IsNullOrEmpty(type))
        {
            typeElement.InnerText = type;
        }
        requestElement.AppendChild(typeElement);

        var parentIdElement = xmlDocument.CreateElement("", "parent_id", SoapRequestBuilder.DpwhNamespace);
        if (!string.IsNullOrEmpty(id))
        {
            parentIdElement.InnerText = id;
        }
        requestElement.AppendChild(parentIdElement);

        xmlDocument.AppendChild(requestElement);

        return new SoapRequestBuilder(_settings)
            .WithRequest(xmlDocument.DocumentElement)
            .Build();
    }
}