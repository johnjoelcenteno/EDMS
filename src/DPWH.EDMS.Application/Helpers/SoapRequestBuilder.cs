using System.Xml;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Domain.Exceptions;

namespace DPWH.EDMS.Application.Helpers;

public class SoapRequestBuilder
{
    public const string SoapNamespace = "http://www.w3.org/2003/05/soap-envelope";
    public const string DpwhNamespace = "https://www.dpwh.gov.ph/";

    private readonly XmlDocument _document;
    private readonly XmlElement _root;

    public SoapRequestBuilder(DpwhIntegrationSettings settings)
    {
        _document = new XmlDocument();

        // Create the SOAP envelope element
        _root = _document.CreateElement("soap12", "Envelope", SoapNamespace);
        _document.AppendChild(_root);

        // Build Credential Header
        var header = BuildHeader(settings);
        _root.AppendChild(header);
    }

    private XmlElement BuildHeader(DpwhIntegrationSettings settings)
    {
        var userName = _document.CreateElement("", "userName", DpwhNamespace);
        userName.InnerText = settings.UserName;

        var password = _document.CreateElement("", "password", DpwhNamespace);
        password.InnerText = settings.Password;

        var userDetails = _document.CreateElement("", "UserDetails", DpwhNamespace);
        userDetails.AppendChild(userName);
        userDetails.AppendChild(password);

        var headerElement = _document.CreateElement("soap12", "Header", SoapNamespace);
        headerElement.AppendChild(userDetails);

        return headerElement;
    }

    public SoapRequestBuilder WithRequest(XmlElement? payload)
    {
        if (payload is null)
        {
            throw new AppException("Xml payload is null");
        }

        var body = _document.CreateElement("soap12", "Body", SoapNamespace);
        var importNode = body.OwnerDocument.ImportNode(payload, true);
        body.AppendChild(importNode);
        _root.AppendChild(body);

        return this;
    }

    public XmlDocument Build() => _document;
}