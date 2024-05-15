using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.GetInspectionRequests;

public record GetInspectionRequestsQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetInspectionRequestsHandler : IRequestHandler<GetInspectionRequestsQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetInspectionRequestsHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetInspectionRequestsQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.InspectionRequestsView
            .Include(i => i.Asset)
            .Include(i => i.RentalRateProperty)
            .Include(i => i.InspectionRequestBuildingComponents)
            .Include(i => i.Documents)
            .Include(i => i.ProjectMonitoring)
            .Include(i => i.InspectionRequestProjectMonitoring)
            .OrderByDescending(i => i.Created)
            .Select(InspectionRequestMappers.MapToModelExpression())
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}