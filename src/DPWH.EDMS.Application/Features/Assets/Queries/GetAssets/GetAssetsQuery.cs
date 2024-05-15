using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Assets.Queries.GetAssets;

public record GetAssetsQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetAssetsHandler : IRequestHandler<GetAssetsQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetAssetsHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.AssetsView
            .Include(a => a.FinancialDetails)
            .Select(AssetMappers.MapToModelExpression())
            .OrderByDescending(b => b.Created)
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}