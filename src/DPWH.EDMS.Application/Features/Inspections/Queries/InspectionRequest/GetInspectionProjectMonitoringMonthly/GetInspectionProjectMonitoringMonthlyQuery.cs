using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionProjectMonitoringMonthly;

public record GetInspectionProjectMonitoringMonthlyQuery(Guid ProjectMonitoringId) : IRequest<GetInspectionProjectMonitoringMonthlyResult>;
internal sealed class GetInspectionProjectMonitoringMonthlyhandler : IRequestHandler<GetInspectionProjectMonitoringMonthlyQuery, GetInspectionProjectMonitoringMonthlyResult>
{
    private readonly IWriteRepository _repository;

    public GetInspectionProjectMonitoringMonthlyhandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetInspectionProjectMonitoringMonthlyResult> Handle(GetInspectionProjectMonitoringMonthlyQuery request, CancellationToken cancellationToken)
    {
        var projectMonitoring = _repository.ProjectMonitoring.Include(x => x.ProjectMonitoringBuildingComponents).Include(x => x.Asset).FirstOrDefault(x => x.Id == request.ProjectMonitoringId)
                ?? throw new AppException("No project monitoring found");

        var inspectionMonthly = await _repository.InspectionRequests
            .Include(x => x.InspectionRequestProjectMonitoring)
            .Include(x => x.InspectionRequestProjectMonitoring.InspectionRequestProjectMonitoringScopes)
            .Where(ir => ir.ProjectMonitoringId == request.ProjectMonitoringId)
            .ToListAsync(cancellationToken);

        return new GetInspectionProjectMonitoringMonthlyResult(projectMonitoring, inspectionMonthly);
    }
}
