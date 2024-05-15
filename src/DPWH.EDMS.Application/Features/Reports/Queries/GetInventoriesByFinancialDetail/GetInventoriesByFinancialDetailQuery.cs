using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByFinancialDetail;

public record GetInventoriesByFinancialDetailQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetInventoriesByFinancialDetailHandler : IRequestHandler<GetInventoriesByFinancialDetailQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetInventoriesByFinancialDetailHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetInventoriesByFinancialDetailQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.AssetsView
            .Select(property => new InventoryReportByFinancialDetails
            {
                PropertyId = property.PropertyId,
                PropertyName = property.Name,
                ZonalValue = property.ZonalValue,
                CreatedBy = property.CreatedBy,
                Created = property.Created,
                LastModifiedBy = property.LastModifiedBy,
                LastModified = property.LastModified
            })
            .OrderByDescending(property => property.Created)
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}