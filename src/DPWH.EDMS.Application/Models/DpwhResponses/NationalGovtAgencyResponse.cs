using System.Xml.Serialization;
using DPWH.EDMS.Application.Helpers;

namespace DPWH.EDMS.Application.Models.DpwhResponses;

[XmlRoot(ElementName = "Envelope", Namespace = SoapRequestBuilder.SoapNamespace)]
public class NationalGovtAgencyResponse : DpwhApiBaseResponse<NationalGovtAgencyResponseBody>
{
    public const string Operation = "ioms_get_national_govt_agency";
}

public class NationalGovtAgencyResponseBody
{
    [XmlElement(ElementName = "ioms_get_national_govt_agencyResponse", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public NationalGovtAgencyContainer? Response { get; set; }
}

public class NationalGovtAgencyContainer
{
    [XmlElement(ElementName = "ioms_get_national_govt_agencyResult", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public NationalGovtAgencyResult? Result { get; set; }
}

public class NationalGovtAgencyResult
{
    [XmlElement(ElementName = "NationalGovtAgency", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public NationalGovtAgency[]? Data { get; set; }
}

public class NationalGovtAgency
{
    [XmlElement(ElementName = "parent_agency_id")]
    public string? AgencyId { get; set; }
    [XmlElement(ElementName = "parent_agency")]
    public string? AgencyName { get; set; }
    [XmlElement(ElementName = "attached_agency_id")]
    public string? AttachedAgencyId { get; set; }
    [XmlElement(ElementName = "attached_agency")]
    public string? AttachedAgencyName { get; set; }
}