using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Maintenance.Mappers;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequests;

public record GetMaintenanceRequest(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;
internal class GetMaintenanceRequestsHandler : IRequestHandler<GetMaintenanceRequest, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetMaintenanceRequestsHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetMaintenanceRequest request, CancellationToken cancellationToken)
    {
        var result = _repository.MaintenanceRequestsView
            .Include(i => i.Asset)
            .Include(i => i.MaintenanceRequestBuildingComponents)
            .AsQueryable()
            .OrderByDescending(i => i.Created)
            .Select(MaintenanceRequestMappers.MapToModelExpression())
            .ToDataSourceResult(request.DataSourceRequest);

        return Task.FromResult(result);
    }
}
