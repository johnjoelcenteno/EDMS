using System.Xml.Serialization;
using DPWH.EDMS.Application.Helpers;

namespace DPWH.EDMS.Application.Models.DpwhResponses;

[XmlRoot(ElementName = "Envelope", Namespace = SoapRequestBuilder.SoapNamespace)]
public class PositionResponse : DpwhApiBaseResponse<PositionResponseBody>
{
    public const string Operation = "pis_get_position_sg";
}

public class PositionResponseBody
{
    [XmlElement(ElementName = "pis_get_position_sgResponse", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public PositionContainer? Response { get; set; }
}

public class PositionContainer
{
    [XmlElement(ElementName = "pis_get_position_sgResult", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public PositionResult? Result { get; set; }
}

public class PositionResult
{
    [XmlElement(ElementName = "PositionSG", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public Position[]? Data { get; set; }
}

public class Position
{
    public string? PosType { get; set; }
    public string? PositionTitle { get; set; }
    public string? SalaryGrade { get; set; }
}