namespace DPWH.EDMS.Web.Client.BFF;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

public class AccessTokenHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navManager;
    private readonly string _accessTokenEndpoint = "AccessToken";

    public AccessTokenHandler(
        AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient, NavigationManager navManager)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _navManager = navManager;
        _httpClient = httpClient;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        try
        {
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                //var responseMessage1 = await _httpClient.GetAsync("r/api/lookups");
                //var responseMessage2 = await _httpClient.GetAsync("/r/api/lookups");

                //var responseContent1 = await responseMessage1.Content.ReadAsStringAsync();
                //var responseContent2 = await responseMessage2.Content.ReadAsStringAsync();

                

                var tokenRes = await _httpClient.GetFromJsonAsync<string>(_accessTokenEndpoint);
                if (!string.IsNullOrEmpty(tokenRes))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenRes);                    
                }
                else
                {
                    Console.WriteLine("Something went wrong on fetching token.");
                }
            }
            else
            {
                Console.WriteLine("Something went wrong on user authentication.");
            }

        }
        catch (Exception ex)
        {
            //_logger.LogError(ex.Message, ex);
            //_navManager.NavigateTo("/401", true);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

