using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsPerPropertyCondition;

public record GetAssetsPerPropertyConditionQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetAssetsPerPropertyConditionHandler : IRequestHandler<GetAssetsPerPropertyConditionQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetAssetsPerPropertyConditionHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetAssetsPerPropertyConditionQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.AssetsView
            .GroupBy(asset => asset.PropertyStatus)
            .Select(g => new AssestPerConditionModel
            {
                NameOfItem = g.Key.ToString(),
                NumberOfItems = g.Count()
            })
            .OrderByDescending(report => report.NumberOfItems)
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}