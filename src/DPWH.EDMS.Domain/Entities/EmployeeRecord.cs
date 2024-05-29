using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain;

public class EmployeeRecord : EntityBase
{
    public static EmployeeRecord Create(
        string firstName,
        string middleName,
        string lastName,
        string office,
        string email,
        string mobileNumber,
        string employeeNumber,
        string regionCentralOffice,
        string districtBureauService,
        string position,
        string designation,
        string employeeId,
        string role,
        string userAccess,
        string department,
        string regionalOfficeRegion,
        string regionalOfficeProvince,
        string districtEngineeringOffice,
        string designationTitle
    )
    {
        return new EmployeeRecord
        {
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            Office = office,
            Email = email,
            MobileNumber = mobileNumber,
            EmployeeNumber = employeeNumber,
            RegionCentralOffice = regionCentralOffice,
            DistrictBureauService = districtBureauService,
            Position = position,
            Designation = designation,
            EmployeeId = employeeId,
            Role = role,
            UserAccess = userAccess,
            Department = department,
            RegionalOfficeRegion = regionalOfficeRegion,
            RegionalOfficeProvince = regionalOfficeProvince,
            DistrictEngineeringOffice = districtEngineeringOffice,
            DesignationTitle = designationTitle,
        };
    }
    public void Update(
        string firstName,
        string middleName,
        string lastName,
        string office,
        string email,
        string mobileNumber,
        string employeeNumber,
        string regionCentralOffice,
        string districtBureauService,
        string position,
        string designation,
        string employeeId,
        string role,
        string userAccess,
        string department,
        string regionalOfficeRegion,
        string regionalOfficeProvince,
        string districtEngineeringOffice,
        string designationTitle
    )
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Office = office;
        Email = email;
        MobileNumber = mobileNumber;
        EmployeeNumber = employeeNumber;
        RegionCentralOffice = regionCentralOffice;
        DistrictBureauService = districtBureauService;
        Position = position;
        Designation = designation;
        EmployeeId = employeeId;
        Role = role;
        UserAccess = userAccess;
        Department = department;
        RegionalOfficeRegion = regionalOfficeRegion;
        RegionalOfficeProvince = regionalOfficeProvince;
        DistrictEngineeringOffice = districtEngineeringOffice;
        DesignationTitle = designationTitle;
    }

    private Guid Id { get; set; }
    private string FirstName { get; set; }
    private string MiddleName { get; set; }
    private string LastName { get; set; }
    private string Office { get; set; }
    private string Email { get; set; }
    private string MobileNumber { get; set; }
    private string EmployeeNumber { get; set; }
    private string RegionCentralOffice { get; set; }
    private string DistrictBureauService { get; set; }
    private string Position { get; set; }
    private string Designation { get; set; }
    private string EmployeeId { get; set; }
    private string Role { get; set; }
    private string UserAccess { get; set; }
    private string Department { get; set; }
    private string RegionalOfficeRegion { get; set; }
    private string RegionalOfficeProvince { get; set; }
    private string DistrictEngineeringOffice { get; set; }
    private string DesignationTitle { get; set; }
}