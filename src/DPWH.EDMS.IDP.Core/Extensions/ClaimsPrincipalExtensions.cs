using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Constants;

namespace DPWH.EDMS.IDP.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userIdString = principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return Guid.Empty;
        }

        return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
    }

    /// <summary>
    /// Returns username which is also the email
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="defaultValue">Set expected default value if email is null</param>
    /// <returns>User's username / email</returns>
    public static string GetUserName(this ClaimsPrincipal principal, string? defaultValue = null)
    {
        defaultValue ??= "Anonymous";
        return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ??
               principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value ??
               defaultValue;
    }

    /// <summary>
    /// Returns user's role. Expectation is that a user can have 0 or 1 role.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public static string? GetRole(this ClaimsPrincipal principal)
    {

        return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ??
               principal.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
    }

    public static string GetRoleLabel(this ClaimsPrincipal principal)
    {
        var role = principal.GetRole();

        return ApplicationRoles.GetDisplayRoleName(role);
    }

    public static string? GetFirstName(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == "firstname")?.Value;
    }

    public static string? GetLastName(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == "lastname")?.Value;
    }
    public static string? GetMiddleInitial(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == "middleinitial")?.Value;
    }

    public static string? GetEmployeeNumber(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == "employee_number")?.Value;
    }

    public static string? GetDisplayName(this ClaimsPrincipal principal, string? defaultValue = null)
    {
        defaultValue ??= "Anonymous";
        var displayName = $"{principal.GetFirstName()} {principal.GetLastName()}";

        return string.IsNullOrWhiteSpace(displayName) ? defaultValue : displayName;
    }
    public static string? GetRegionalOffice(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == "regional_office")?.Value;
    }

    public static string? GetImplementingOffice(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == "implementing_office")?.Value;
    }

    public static string? GetOffice(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == "office")?.Value;
    }

    public static bool IsFromCentralOffice(this ClaimsPrincipal principal)
    {
        return principal.GetRegionalOffice() is "Central Office";
    }
}