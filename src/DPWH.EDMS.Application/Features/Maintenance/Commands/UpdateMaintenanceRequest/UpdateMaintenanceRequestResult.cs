using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.UpdateMaintenanceRequest;
public record UpdateMaintenanceRequestResult
{
    public UpdateMaintenanceRequestResult(MaintenanceRequest entity)
    {
        Id = entity.Id;
    }
    public Guid Id { get; set; }
}
