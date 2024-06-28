using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Licenses.Queries;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Services;

public interface IUserAccessLevelService
{
    Task<GetLicenseStatusResult> GetLicenseStatus(CancellationToken cancellationToken);
}

public class UserAccessLevelService : IUserAccessLevelService
{
    private const string LicenseMaxLimitConfigName = "LicenseMaxLimit";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IReadRepository _readRepository;

    public UserAccessLevelService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IReadRepository readRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _readRepository = readRepository;
    }

    public async Task<GetLicenseStatusResult> GetLicenseStatus(CancellationToken cancellationToken)
    {
        //get limit from ConfigSettings - we can change this soon
        var limit = await _readRepository.ConfigSettingsView
            .FirstOrDefaultAsync(x => x.Name == LicenseMaxLimitConfigName, cancellationToken);

        if (limit is null)
        {
            throw new AppException("LicenseMaxLimit not configured");
        }

        var maxLimit = int.Parse(limit.Value);

        //get all users with roles that requires license
        var licenseRoles = await _roleManager.Roles
            .Where(r => !ApplicationPolicies.NoLicenseUsers.Contains(r.Name) && ApplicationPolicies.RequireActiveRoles.Contains(r.Name))
            .Select(r => new SimpleKeyValue(r.Id, r.Name))
            .ToListAsync(cancellationToken);

        var userList = new List<ApplicationUser>();

        foreach (var role in licenseRoles.Select(r => new Claim("role", r.Name)))
        {

            var users = await _userManager.GetUsersForClaimAsync(role);

            if (!users.Any())
            {
                continue;
            }

            userList.AddRange(users);
        }

        var endUsers = await _userManager.GetUsersForClaimAsync(new Claim("role", ApplicationRoles.EndUser.ToString()));

        return new GetLicenseStatusResult(maxLimit, maxLimit - userList.Count, EndUsersCount: endUsers.Count);
    }
}