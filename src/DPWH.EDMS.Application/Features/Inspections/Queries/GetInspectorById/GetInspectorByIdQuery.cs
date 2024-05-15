using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.IDP.Core.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.GetInspectorById;

public record GetInspectorByIdQuery(string EmployeeId) : IRequest<GetInspectorByIdResult?>;

internal sealed class GetInspectorByIdHandler : IRequestHandler<GetInspectorByIdQuery, GetInspectorByIdResult?>
{
    private readonly IReadAppIdpRepository _readRepository;

    public GetInspectorByIdHandler(IReadAppIdpRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<GetInspectorByIdResult?> Handle(GetInspectorByIdQuery request, CancellationToken cancellationToken)
    {
        var users = (from user in _readRepository.UsersView
                     where user.EmployeeInfo.EmployeeId == request.EmployeeId
                     && _readRepository.UserClaimsView
                                  .Any(u => u.UserId == user.Id && u.ClaimType == "role" && u.ClaimValue == "dpwh_inspector")

                     select new
                     {
                         UserId = user.Id,
                         user.UserName,
                         user.Email,
                         user.UserBasicInfo.FirstName,
                         user.UserBasicInfo.LastName,
                         user.EmployeeInfo.EmployeeId,
                         MobileNumber = user.PhoneNumber,
                         RoleNames = _readRepository.UserClaimsView
                             .Where(u => u.UserId == user.Id && (u.ClaimType == "role" || u.ClaimType == "account_status"))
                             .Select(uc => uc.ClaimValue)
                             .ToList(),
                         user.EmployeeInfo.Department,
                         user.EmployeeInfo.Position,
                         user.EmployeeInfo.RegionalOfficeRegion,
                         user.EmployeeInfo.RegionalOfficeProvince,
                         user.EmployeeInfo.DistrictEngineeringOffice,
                         user.EmployeeInfo.DesignationTitle,
                         user.Created,
                         user.CreatedBy
                     }).Select(p => new GetInspectorByIdResult
                     {
                         Id = p.UserId,
                         UserName = p.UserName,
                         Email = p.Email,
                         FirstName = p.FirstName,
                         LastName = p.LastName,
                         EmployeeId = p.EmployeeId,
                         MobileNumber = p.MobileNumber,
                         Role = string.Join(',', p.RoleNames),
                         UserAccess = ApplicationRoles.GetDisplayRoleName(p.RoleNames.FirstOrDefault(), "None"),
                         Department = p.Department,
                         Position = p.Position,
                         RegionalOfficeRegion = p.RegionalOfficeRegion,
                         RegionalOfficeProvince = p.RegionalOfficeProvince,
                         DistrictEngineeringOffice = p.DistrictEngineeringOffice,
                         DesignationTitle = p.DesignationTitle,
                         CreatedDate = p.Created,
                         CreatedBy = p.CreatedBy

                     }).FirstOrDefaultAsync(cancellationToken);

        return users;

    }
}
