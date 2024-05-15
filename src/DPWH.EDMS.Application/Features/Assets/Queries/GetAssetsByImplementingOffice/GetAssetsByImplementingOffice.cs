using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByImplementingOffice;

public record GetAssetsByImplementingOffice(DataSourceRequest Request) : IRequest<DataSourceResult>;

internal sealed class GetAssetsByImplementingOfficeHandler : IRequestHandler<GetAssetsByImplementingOffice, DataSourceResult>
{
    private readonly IReadRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private readonly ILogger<GetAssetsByImplementingOfficeHandler> _logger;

    public GetAssetsByImplementingOfficeHandler(IReadRepository repository, ClaimsPrincipal principal, ILogger<GetAssetsByImplementingOfficeHandler> logger)
    {
        _repository = repository;
        _principal = principal;
        _logger = logger;
    }

    public Task<DataSourceResult> Handle(GetAssetsByImplementingOffice request, CancellationToken cancellationToken)
    {
        var assets = _repository.AssetsView.Include(a => a.FinancialDetails).AsQueryable();

        if (_principal.IsInRole(ApplicationRoles.SuperAdmin) || _principal.IsInRole(ApplicationRoles.SystemAdmin))
        {
            var assetsList = assets.Select(AssetMappers.MapToModelExpression());
            return Task.FromResult(assetsList.OrderByDescending(b => b.Created).ToDataSourceResult(request.Request));
        }

        if (_principal.IsFromCentralOffice())
        {
            assets = assets.Where(a => a.RequestingOffice == _principal.GetOffice());
        }
        else
        {
            if (!string.IsNullOrEmpty(_principal.GetImplementingOffice()))
            {
                assets = assets.Where(a => a.ImplementingOffice == _principal.GetImplementingOffice());
            }
            else
            {
                _logger.LogWarning("User has no valid district/bureau/service {@User}", _principal.GetUserName());
            }
        }

        var assetsDtos = assets.Select(AssetMappers.MapToModelExpression());

        var result = assetsDtos.OrderByDescending(b => b.Created).ToDataSourceResult(request.Request);

        return Task.FromResult(result);

    }
}


