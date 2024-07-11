using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.IDP.Core.Entities;

public class EmployeeInfo
{
    private EmployeeInfo(
        string? employeeId,
        string? department,
        string? position,
        string? regionalOfficeRegion,
        string? regionalOfficeProvince,
        string? districtEngineeringOffice,
        string? designationTitle,
        string? office)
    {
        EmployeeId = employeeId;
        Department = department;
        Position = position;
        RegionalOfficeRegion = regionalOfficeRegion;
        RegionalOfficeProvince = regionalOfficeProvince;
        DistrictEngineeringOffice = districtEngineeringOffice;
        DesignationTitle = designationTitle;
        Office = office;
    }

    public static EmployeeInfo Create(
        string? employeeId,
        string? department,
        string? position,
        string? regionalOfficeRegion,
        string? regionalOfficeProvince,
        string? districtEngineeringOffice,
        string? designationTitle,
        string? office)
    {
        return new EmployeeInfo(
            employeeId,
            department,
            position,
            regionalOfficeRegion,
            regionalOfficeProvince,
            districtEngineeringOffice,
            designationTitle, office);
    }

    public string? Id { get; private set; }
    [StringLength(50)]
    public string? EmployeeId { get; private set; }
    [StringLength(100)]
    public string? Department { get; private set; }
    [StringLength(100)]
    public string? Position { get; private set; }
    [StringLength(100)]
    public string? RegionalOfficeRegion { get; private set; }
    [StringLength(100)]
    public string? RegionalOfficeProvince { get; private set; }
    [StringLength(100)]
    public string? BureauServiceRoDeoId { get; private set; }
    [StringLength(100)]
    public string? BureauServiceRoDeo { get; private set; }
    [StringLength(100)]
    public string? DistrictEngineeringOffice { get; private set; }
    [StringLength(100)]
    public string? DesignationGroup { get; private set; }
    [StringLength(100)]
    public string? DesignationCode { get; private set; }
    [StringLength(100)]
    public string? DesignationTitle { get; private set; }
    [StringLength(100)]
    public string? DesignationOfficeId { get; private set; }
    [StringLength(100)]
    public string? DesignationOffice { get; private set; }
    [StringLength(100)]
    public string? Office { get; private set; }
}