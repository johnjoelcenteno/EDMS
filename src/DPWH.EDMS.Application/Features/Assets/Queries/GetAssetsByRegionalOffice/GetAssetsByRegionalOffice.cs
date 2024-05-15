using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByRegionalOffice;

public record GetAssetsByRegionalOffice(DataSourceRequest Request) : IRequest<DataSourceResult>;

internal sealed class GetAssetsByRegionalOfficeHandler : IRequestHandler<GetAssetsByRegionalOffice, DataSourceResult>
{
    private readonly IReadRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private readonly ILogger<GetAssetsByRegionalOfficeHandler> _logger;

    public GetAssetsByRegionalOfficeHandler(IReadRepository repository, ClaimsPrincipal principal, ILogger<GetAssetsByRegionalOfficeHandler> logger)
    {
        _repository = repository;
        _principal = principal;
        _logger = logger;
    }

    public Task<DataSourceResult> Handle(GetAssetsByRegionalOffice request, CancellationToken cancellationToken)
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
            if (!string.IsNullOrEmpty(_principal.GetOffice()))
            {
                assets = assets.Where(a => a.RequestingOffice == _principal.GetOffice());
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


