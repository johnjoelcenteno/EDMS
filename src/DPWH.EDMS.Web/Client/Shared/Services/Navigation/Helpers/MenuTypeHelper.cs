using DPWH.EDMS.IDP.Core.Constants;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation.Helpers;

public enum MenuType
{
    [Display(Name = "Home")]
    Home,

    [Display(Name = "Records Management")]
    RecordsManagement,

    [Display(Name = "Request Management")]
    RequestManagement,

    [Display(Name = "User Management")]
    UserManagement,

    [Display(Name = "Reports and Analytics")]
    ReportsAndAnalytics,

    #region Reports
    [Display(Name = "Reports")]
    Reports,

    [Display(Name = "Users")]
    ReportsUsers,

    [Display(Name = "Records Management")]
    ReportsRecordsManagement,

    [Display(Name = "Request Management")]
    ReportsRequestManagement,

    [Display(Name = "User Management")]
    ReportsUserManagement,

    [Display(Name = "System")]
    ReportsSystem,
    #endregion

    #region Audit Trail
    [Display(Name = "Audit Trail")]
    AuditTrail,

    [Display(Name = "User Activity")]
    AuditTrailUserActivity,    

    [Display(Name = "Request Management")]
    AuditTrailRequestManagement,

    [Display(Name = "Records Management")]
    AuditTrailRecordsManagement,

    [Display(Name = "User Management")]
    AuditTrailUserManagement,

    [Display(Name = "Data Library")]
    AuditTrailDataLibrary,
    #endregion

    [Display(Name = "Data Library")]
    DataLibrary,

    [Display(Name = "My Records")]
    MyRecords,

    [Display(Name = "My Requests")]
    MyRequests,

    [Display(Name = "Profile")]
    Profile,

    [Display(Name = "Settings")]
    Settings,

    [Display(Name = "Support")]
    Support
}
public static class MenuTypeExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()?
                        .GetName() ?? enumValue.ToString();
    }

    public static readonly List<string> HomeRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.Manager, ApplicationRoles.ITSupport, ApplicationRoles.Staff
    };

    public static readonly List<string> RecordsManagementRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.Manager, ApplicationRoles.Staff
    };

    public static readonly List<string> RequestManagementRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.Manager, ApplicationRoles.Staff
    };

    public static readonly List<string> UserManagementRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.ITSupport
    };

    public static readonly List<string> ReportsAndAnalyticsRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin
    };

    public static readonly List<string> DataLibraryRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.ITSupport
    };

    public static readonly List<string> MyRecordsRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.EndUser, ApplicationRoles.Manager, ApplicationRoles.Staff
    };

    public static readonly List<string> MyRequestsRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.EndUser, ApplicationRoles.Manager, ApplicationRoles.Staff
    };

    public static readonly List<string> ProfileRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin
    };

    public static readonly List<string> SettingsRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin
    };

    public static readonly List<string> SupportRoles = new List<string>
    {
        ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin
    };

}

