using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetBuildingComponentsByRequestNumber;

public record GetBuildingComponentsByRequestNumberQuery(string RequestNumber, string Purpose) : IRequest<GetBuildingComponentsByRequestNumberResult>;
internal sealed class GetBuildingComponentsByRequestNumber : IRequestHandler<GetBuildingComponentsByRequestNumberQuery, GetBuildingComponentsByRequestNumberResult>
{
    private readonly IReadRepository _repository;

    public GetBuildingComponentsByRequestNumber(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetBuildingComponentsByRequestNumberResult> Handle(GetBuildingComponentsByRequestNumberQuery request, CancellationToken cancellationToken)
    {
        ProjectMonitoring projectMonitoring = null;
        MaintenanceRequest maintenanceRequest = null;

        if (request.Purpose == "Priority List Inspection")
        {
            maintenanceRequest = await _repository.MaintenanceRequestsView.Include(x => x.MaintenanceRequestBuildingComponents).FirstOrDefaultAsync(x => x.RequestNumber == request.RequestNumber)
                ?? throw new AppException("No maintenance request found");
        }
        else if (request.Purpose == "Project Monitoring")
        {
            projectMonitoring = await _repository.ProjectMonitoringView.Include(x => x.ProjectMonitoringBuildingComponents).FirstOrDefaultAsync(x => x.ContractId == request.RequestNumber)
                ?? throw new AppException("No project monitoring found");
        }

        return new GetBuildingComponentsByRequestNumberResult(projectMonitoring, maintenanceRequest);
    }
}
