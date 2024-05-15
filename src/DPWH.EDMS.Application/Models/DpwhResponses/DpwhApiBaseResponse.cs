using System.Xml.Serialization;
using DPWH.EDMS.Application.Helpers;

namespace DPWH.EDMS.Application.Models.DpwhResponses;

public class DpwhApiBaseResponse<T>
{
    [XmlElement(ElementName = "Body", Namespace = SoapRequestBuilder.SoapNamespace)]
    public T? Body { get; set; }
}