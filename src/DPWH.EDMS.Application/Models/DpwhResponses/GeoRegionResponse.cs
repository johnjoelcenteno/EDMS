using System.Xml.Serialization;
using DPWH.EDMS.Application.Helpers;

namespace DPWH.EDMS.Application.Models.DpwhResponses;

[XmlRoot(ElementName = "Envelope", Namespace = SoapRequestBuilder.SoapNamespace)]
public class GeoRegionResponse : DpwhApiBaseResponse<GeoRegionResponseBody>
{
    public const string Operation = "loc_get_reg_prov_cm_brgy";
    public GeoRegion[] GetLocations() => Body?.Response?.Result?.Data ?? [];
}

public class GeoRegionResponseBody
{
    [XmlElement(ElementName = "loc_get_reg_prov_cm_brgyResponse", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public GeoRegionContainer? Response { get; set; }
}

public class GeoRegionContainer
{
    [XmlElement(ElementName = "loc_get_reg_prov_cm_brgyResult", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public GeoRegionResult? Result { get; set; }
}

public class GeoRegionResult
{
    [XmlElement(ElementName = "GeoLocation", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public GeoRegion[]? Data { get; set; }
}

public class GeoRegion
{
    [XmlElement(ElementName = "my_id")]
    public string? MyId { get; set; }

    [XmlElement(ElementName = "my_id_admin")]
    public string? MyIdAdmin { get; set; }

    [XmlElement(ElementName = "parent_id")]
    public string? ParentId { get; set; }

    [XmlElement(ElementName = "admin_area_name_reg")]
    public string? Name { get; set; }

    [XmlElement(ElementName = "admin_area_name_type")]
    public string? Type { get; set; }
}