namespace DPWH.EDMS.IDP.Core.Constants;

using static ApplicationRoles;

public static class ApplicationPolicies
{
    public static string[] AdminOnly => new[] { SystemAdmin, SuperAdmin };
    public static string[] RequireActiveRoles => new[] { SystemAdmin, SuperAdmin, Manager, EndUser, ITSupport, Inspector, Requestor };
    public static string[] RequireForUserManagement => new[] { SystemAdmin, SuperAdmin, ITSupport };
    public static readonly string[] NoLicenseUsers = { SystemAdmin, Inspector, Requestor, EndUser };

    public static readonly IReadOnlyDictionary<string, string[]> GetAll = new Dictionary<string, string[]>{
        { nameof(AdminOnly), AdminOnly },
        { nameof(NoLicenseUsers), NoLicenseUsers }
    };
}