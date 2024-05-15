using InspectionRequestEntity = DPWH.EDMS.Domain.Entities.InspectionRequest;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateBuildingComponent;

public record UpdateBuildingComponentResult
{
    public UpdateBuildingComponentResult(InspectionRequestEntity entity)
    {
        Id = entity.Id;
    }
    public Guid Id { get; set; }
}