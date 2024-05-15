using InspectionRequestEntity = DPWH.EDMS.Domain.Entities.InspectionRequest;


namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateInspectionRequest;

public record UpdateInspectionRequestResult
{
    public UpdateInspectionRequestResult(InspectionRequestEntity entity)
    {
        Id = entity.Id;
    }
    public Guid Id { get; set; }
}
