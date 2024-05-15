using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.DeleteBuildingComponent;

public record DeleteBuildingComponentResult
{
    public DeleteBuildingComponentResult(InspectionRequestBuildingComponent entity)
    {
        Id = entity.Id;
    }
    public Guid Id { get; set; }
}
