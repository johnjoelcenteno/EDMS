using DPWH.EDMS.Domain;

namespace DPWH.EDMS.Application;

public class CreateUpdateEmployeeModel
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Office { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string EmployeeNumber { get; set; }
    public string RegionCentralOffice { get; set; }
    public string DistrictBureauService { get; set; }
    public string Position { get; set; }
    public string Designation { get; set; }
    public string EmployeeId { get; set; }
    public string Role { get; set; }
    public string UserAccess { get; set; }
    public string Department { get; set; }
    public string RegionalOfficeRegion { get; set; }
    public string RegionalOfficeProvince { get; set; }
    public string DistrictEngineeringOffice { get; set; }
    public string DesignationTitle { get; set; }
}

public static class CreateUpdateEmployeeMappers
{
    public static EmployeeRecord MapModelToEntity(CreateUpdateEmployeeModel model)
    {
        return EmployeeRecord.Create(
            model.FirstName,
            model.MiddleName,
            model.LastName,
            model.Office,
            model.Email,
            model.MobileNumber,
            model.EmployeeNumber,
            model.RegionCentralOffice,
            model.DistrictBureauService,
            model.Position,
            model.Designation,
            model.EmployeeId,
            model.Role,
            model.UserAccess,
            model.Department,
            model.RegionalOfficeRegion,
            model.RegionalOfficeProvince,
            model.DistrictEngineeringOffice,
            model.DesignationTitle
        );
    }
}
