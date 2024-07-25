using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Licenses.Queries;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Services;

public interface IUserAccessLevelService
{
    Task<GetLicenseStatusResult> GetLicenseStatus(CancellationToken cancellationToken);
}

public class UserAccessLevelService : IUserAccessLevelService
{
    private const string LicenseMaxLimitConfigName = "LicenseMaxLimit";
    
    private readonly IReadRepository _readRepository;
    private readonly IReadAppIdpRepository _readIdpRepository;

    public UserAccessLevelService(IReadRepository readRepository, IReadAppIdpRepository readIdpRepository)
    {        
        _readRepository = readRepository;
        _readIdpRepository = readIdpRepository;
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

        var allUsers = await _readIdpRepository.ViewUserAccess            
            .Select(r => new SimpleKeyValue(r.Id, r.UserRole))
            .ToListAsync(cancellationToken);

        var allLicenseUsers = allUsers.Count(r => ApplicationPolicies.RequireLicenseRoles.Contains(r.Name));

        return new GetLicenseStatusResult(maxLimit, maxLimit - allLicenseUsers, EndUsersCount: allUsers.Count);
    }
}