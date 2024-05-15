using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByFundingHistory;

public record GetInventoriesByFundingHistoryQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetInventoriesByFundingHistoryHandler : IRequestHandler<GetInventoriesByFundingHistoryQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetInventoriesByFundingHistoryHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetInventoriesByFundingHistoryQuery request, CancellationToken cancellationToken)
    {
        var result = _repository
            .AssetsView
            .SelectMany(group => group.FinancialDetailsDocuments, (asset, financialDetailsDocument) => new InventoryReportByFundingHistoryResult
            {
                BuildingId = asset.BuildingId,
                BuildingName = asset.Name,
                PropertyId = asset.PropertyId,
                YearFunded = financialDetailsDocument.YearFunded,
                Allocation = financialDetailsDocument.Allocation,
                Uri = financialDetailsDocument.Uri,
                Created = asset.Created,
                CreatedBy = asset.CreatedBy,
                LastModified = asset.LastModified,
                LastModifiedBy = asset.LastModifiedBy
            })
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}