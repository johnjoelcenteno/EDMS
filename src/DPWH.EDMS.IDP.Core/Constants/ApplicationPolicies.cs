namespace DPWH.EDMS.IDP.Core.Constants;

using static ApplicationRoles;

public static class ApplicationPolicies
{
    public static string[] AdminOnly => new[] { SystemAdmin, SuperAdmin };
    public static string[] RequireActiveRoles => new[] { SystemAdmin, SuperAdmin, Manager, EndUser, ITSupport, Staff, Deactivated };
    public static string[] RequireLicenseRoles => new[] { SuperAdmin, ITSupport, Manager, Staff };
    public static string[] RequireForUserManagement => new[] { SystemAdmin, SuperAdmin, ITSupport };
    public static readonly string[] NoLicenseUsers = { SystemAdmin, EndUser };

    public static readonly IReadOnlyDictionary<string, string[]> GetAll = new Dictionary<string, string[]>{
        { nameof(AdminOnly), AdminOnly },
        { nameof(NoLicenseUsers), NoLicenseUsers }
    };
}