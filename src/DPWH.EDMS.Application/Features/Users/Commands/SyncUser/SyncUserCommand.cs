using System.Security.Claims;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Users.Commands.SyncUser;

public record SyncUserCommand(Employee EmployeeData) : IRequest<SyncUserResult>;

internal sealed class SyncUserHandler : IRequestHandler<SyncUserCommand, SyncUserResult>
{
    private readonly ILogger<SyncUserHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ClaimsPrincipal _principal;
    private readonly IWriteRepository _repository;
    private const string Employee = "Employee";

    public SyncUserHandler(
        IWriteRepository repository,
        ILogger<SyncUserHandler> logger,
        UserManager<ApplicationUser> userManager,
        ClaimsPrincipal principal)
    {
        _repository = repository;
        _logger = logger;
        _userManager = userManager;
        _principal = principal;
    }

    public async Task<SyncUserResult> Handle(SyncUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sync user from PIS API {@Command}", command);
        var syncResult = "Success";
        var syncDescription = "";

        try
        {
            var model = command.EmployeeData;
            var inputUserName = $"{model.NetworkId}@dpwh.gov.ph";

            //check if user already exist            
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == inputUserName, cancellationToken);

            if (user is not null)
            {
                _logger.LogInformation("Update user with data from PIS API");

                if (user.UserName != inputUserName) user.UserName = inputUserName;

                //update properties
                var userBasicInfo = UserBasicInfo.Create(
                    model.FirstName,
                    model.MiddleInitial,
                    model.FamilyName);

                var employeeInfo = EmployeeInfo.Create(
                    model.EmployeeId,
                    model.PlantillaOfficeName,
                    model.PlantillaPosition,
                    model.CentralOrRegionName,
                    model.BureauServiceRoDeoName,
                    model.BureauServiceRoDeoName,
                    model.DesignationTitle);

                user.Email = $"{model.NetworkId}@dpwh.gov.ph";
                user.Update(
                    userBasicInfo,
                    employeeInfo,
                    _principal.GetUserName());

                var identityResult = await _userManager.UpdateAsync(user);
                syncDescription = $"Successfully synced employee {user.EmployeeInfo.EmployeeId}.";

                if (!identityResult.Succeeded)
                {
                    syncResult = "Error";
                    syncDescription = identityResult.Errors.First().Description;
                    throw new Exception(identityResult.Errors.First().Description);
                }

                return new SyncUserResult
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    FirstName = user.UserBasicInfo?.FirstName,
                    LastName = user.UserBasicInfo?.LastName,
                    MobileNumber = user.PhoneNumber,
                    Department = user.EmployeeInfo.Department,
                    Position = user.EmployeeInfo.Position,
                    RegionalOfficeRegion = user.EmployeeInfo.RegionalOfficeRegion,
                    RegionalOfficeProvince = user.EmployeeInfo.RegionalOfficeProvince,
                    DistrictEngineeringOffice = user.EmployeeInfo.DistrictEngineeringOffice,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.Created
                };
            }
            else
            {
                throw new AppException("User does not exist");
            }
        }
        catch (Exception ex)
        {
            syncResult = "Error";
            syncDescription = ex.Message;
        }
        finally
        {
            var syncLog = DataSyncLog.Create(Employee, syncResult, syncDescription, _principal.GetUserName());
            await _repository.DataSyncLogs.AddAsync(syncLog, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        _logger.LogError("User does not exist");
        throw new AppException("User does not exist");
    }
}