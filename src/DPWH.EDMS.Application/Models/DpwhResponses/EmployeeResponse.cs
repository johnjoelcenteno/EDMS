using System.Xml.Serialization;
using DPWH.EDMS.Application.Helpers;

namespace DPWH.EDMS.Application.Models.DpwhResponses;

[XmlRoot(ElementName = "Envelope", Namespace = SoapRequestBuilder.SoapNamespace)]
public class EmployeeResponse : DpwhApiBaseResponse<EmployeeResponseBody>
{
    public const string Operation = "pis_get_employee";
}

public class EmployeeResponseBody
{
    [XmlElement(ElementName = "pis_get_employeeResponse", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public EmployeeContainer? Container { get; set; }
}

[XmlType(AnonymousType = true, Namespace = SoapRequestBuilder.DpwhNamespace)]
public class EmployeeContainer
{
    [XmlElement(ElementName = "pis_get_employeeResult", Namespace = SoapRequestBuilder.DpwhNamespace)]
    public Employee? Data { get; set; }
}

[XmlType(AnonymousType = true, Namespace = SoapRequestBuilder.DpwhNamespace)]
public class Employee
{
    [XmlElement(ElementName = "employee_id")]
    public string? EmployeeId { get; set; }
    [XmlElement(ElementName = "network_id")]
    public string? NetworkId { get; set; }
    [XmlElement(ElementName = "person_family_name")]
    public string? FamilyName { get; set; }
    [XmlElement(ElementName = "person_forename")]
    public string? FirstName { get; set; }
    [XmlElement(ElementName = "person_middle_initial_1")]
    public string? MiddleInitial { get; set; }
    [XmlElement(ElementName = "position_code")]
    public string? PositionCode { get; set; }
    [XmlElement(ElementName = "plantilla_position")]
    public string? PlantillaPosition { get; set; }
    [XmlElement(ElementName = "plantilla_office_id")]
    public string? PlantillaOfficeId { get; set; }
    [XmlElement(ElementName = "plantilla_office_name")]
    public string? PlantillaOfficeName { get; set; }
    [XmlElement(ElementName = "bureau_service_ro_deo_id")]
    public string? BureauServiceRoDeoId { get; set; }
    [XmlElement(ElementName = "bureau_service_ro_deo_name")]
    public string? BureauServiceRoDeoName { get; set; }
    [XmlElement(ElementName = "central_or_region_id")]
    public string? CentralOrRegionId { get; set; }
    [XmlElement(ElementName = "central_or_region_name")]
    public string? CentralOrRegionName { get; set; }
    [XmlElement(ElementName = "designation_group")]
    public string? DesignationGroup { get; set; }
    [XmlElement(ElementName = "designation_code")]
    public string? DesignationCode { get; set; }
    [XmlElement(ElementName = "designation_title")]
    public string? DesignationTitle { get; set; }
    [XmlElement(ElementName = "designation_office_id")]
    public string? DesignationOfficeId { get; set; }
    [XmlElement(ElementName = "designation_office_name")]
    public string? DesignationOfficeName { get; set; }
    [XmlElement(ElementName = "designation_start_date")]
    public string? DesignationStartDate { get; set; }
    [XmlElement(ElementName = "designation_end_date")]
    public string? DesignationStartEnd { get; set; }
}