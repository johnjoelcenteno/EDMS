using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Mappers;
using DPWH.EDMS.Application.Features.Maintenance.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoring;

public record GetProjectMonitoringQuery(DataSourceRequest Request) : IRequest<DataSourceResult>;
internal sealed class GetProjectMonitoringHandler : IRequestHandler<GetProjectMonitoringQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetProjectMonitoringHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetProjectMonitoringQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.ProjectMonitoringView
            .Include(i => i.Asset)
            .Include(i => i.ProjectMonitoringBuildingComponents)
            .OrderByDescending(i => i.Created)
            .Select(ProjectMonitoringMappers.MapToModelExpression())
            .ToDataSourceResult(request.Request.FixSerialization());

        return Task.FromResult(result);
    }
}
