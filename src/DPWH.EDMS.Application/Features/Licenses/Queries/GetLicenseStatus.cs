using DPWH.EDMS.Application.Services;
using MediatR;

namespace DPWH.EDMS.Application.Features.Licenses.Queries;

public record GetLicenseStatusQuery : IRequest<GetLicenseStatusResult>;

internal sealed class GetLicenseStatusQueryHandler : IRequestHandler<GetLicenseStatusQuery, GetLicenseStatusResult>
{
    private readonly IUserAccessLevelService _userAccessLevelService;

    public GetLicenseStatusQueryHandler(IUserAccessLevelService userAccessLevelService)
    {
        _userAccessLevelService = userAccessLevelService;
    }

    public async Task<GetLicenseStatusResult> Handle(GetLicenseStatusQuery request, CancellationToken cancellationToken)
    {
        return await _userAccessLevelService.GetLicenseStatus(cancellationToken);
    }
}
