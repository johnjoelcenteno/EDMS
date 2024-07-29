namespace DPWH.EDMS.IDP.Core.Constants;

public static class ApplicationRoles
{
    private const string RolePrefix = "dpwh_edms_";

    public const string SystemAdmin = "system_admin";
    public const string SuperAdmin = $"{RolePrefix}superadmin";
    public const string Manager = $"{RolePrefix}manager";
    public const string EndUser = $"{RolePrefix}enduser";
    public const string ITSupport = $"{RolePrefix}it_support";
    public const string Staff = $"{RolePrefix}staff";
    public const string Deactivated = $"{RolePrefix}deactivated";    

    public static readonly IReadOnlyDictionary<string, string> UserAccessMapping = new Dictionary<string, string>{
            { "system_admin", "system_admin" },
            { SuperAdmin, "Super Admin" },
            { Manager, "Manager" },
            { ITSupport, "IT Support"},
            { EndUser, "End User"},
            { Staff, "Staff"},
            { Deactivated, "Deactivated" }            
        };

    public static string GetDisplayRoleName(string? roleClaim, string? defaultValue = null)
    {
        defaultValue ??= "Unauthorized";

        if (string.IsNullOrEmpty(roleClaim))
            return defaultValue;

        return UserAccessMapping.TryGetValue(roleClaim, out var displayName) ? displayName : defaultValue;
    }

    public static IReadOnlyDictionary<string, string> AssignableRoles => UserAccessMapping
        .Where(m => m.Key != SystemAdmin && m.Key != Deactivated)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}