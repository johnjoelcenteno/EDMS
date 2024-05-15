using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DPWH.EDMS.IDP.Core.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public sealed class ApplicationUser : IdentityUser
{
    private ApplicationUser()
    {
    }

    public static ApplicationUser Create(
        string subject,
        string? username,
        string? email,
        UserBasicInfo? userBasicInfo,
        string createdBy)
    {
        return new ApplicationUser
        {
            Id = subject,
            UserName = username ?? subject,
            Email = email,
            UserBasicInfo = userBasicInfo,
            CreatedBy = createdBy
        };
    }

    public static ApplicationUser Create(
        string username,
        string email,
        string mobileNumber,
        UserBasicInfo? userBasicInfo,
        EmployeeInfo? employeeInfo,
        string createdBy,
        bool emailConfirmed = true)
    {
        return new ApplicationUser
        {
            UserName = username,
            Email = email,
            PhoneNumber = mobileNumber,
            UserBasicInfo = userBasicInfo,
            EmployeeInfo = employeeInfo,
            EmailConfirmed = emailConfirmed,
            CreatedBy = createdBy,
            Created = DateTimeOffset.Now
        };
    }

    public void Update(UserBasicInfo? userBasicInfo, EmployeeInfo? employeeInfo, string modifiedBy)
    {
        UserBasicInfo = userBasicInfo;
        EmployeeInfo = employeeInfo;

        SetModified(modifiedBy);
    }

    public UserBasicInfo? UserBasicInfo { get; private set; }
    public EmployeeInfo? EmployeeInfo { get; private set; }

    public DateTimeOffset Created { get; private set; }
    [StringLength(100)]
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? LastModified { get; private set; }
    [StringLength(100)]
    public string? LastModifiedBy { get; private set; }

    private void SetModified(string updatedBy)
    {
        LastModifiedBy = updatedBy;
        LastModified = DateTimeOffset.Now;
    }
}
