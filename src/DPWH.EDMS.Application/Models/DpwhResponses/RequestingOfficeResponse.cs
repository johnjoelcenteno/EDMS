using System.Xml.Serialization;
using DPWH.EDMS.Application.Helpers;

namespace DPWH.EDMS.Application.Models.DpwhResponses;

[XmlRoot(ElementName = "Envelope", Namespace = SoapRequestBuilder.SoapNamespace)]
public class RequestingOfficeResponse : DpwhApiBaseResponse<RequestingOfficeResponseBody>
{
    public const string Operation = "pis_get_requesting_office";
}

public class RequestingOfficeResponseBody
{
    [XmlElement(ElementName = "pis_get_requesting_officeResponse", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public RequestingOfficeContainer? Response { get; set; }
}

public class RequestingOfficeContainer
{
    [XmlElement(ElementName = "pis_get_requesting_officeResult", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public RequestingOfficeResult? Result { get; set; }
}

public class RequestingOfficeResult
{
    [XmlElement(ElementName = "ImplementingOffice", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public ImplementingOffice[]? Data { get; set; }
}

public class ImplementingOffice
{
    [XmlElement(ElementName = "OfficeID")]
    public string? OfficeId { get; set; }
    public string? OfficeName { get; set; }
    [XmlElement(ElementName = "SubOfficeID")]
    public string? SubOfficeId { get; set; }
    public string? SubOfficeName { get; set; }
}