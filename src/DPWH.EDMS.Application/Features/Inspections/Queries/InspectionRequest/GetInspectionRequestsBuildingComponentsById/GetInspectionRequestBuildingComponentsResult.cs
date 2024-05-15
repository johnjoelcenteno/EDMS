using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequestsBuildingComponentsById;

public record GetInspectionRequestBuildingComponentsResult
{
    public GetInspectionRequestBuildingComponentsResult(InspectionRequestBuildingComponent entity)
    {
        InspectionRequestId = entity.InspectionRequestId;
        Category = entity.Category;
        SubComponents = new SubComponent
        {
            SubCategory = entity.SubCategory,
            ForRepair = entity.ForRepair,
            Rating = entity.Rating,
            Particular = entity.Particular,
        };
    }

    public Guid InspectionRequestId { get; set; }
    public string Category { get; set; }
    public SubComponent SubComponents { get; set; }
    public class SubComponent
    {
        public string? SubCategory { get; set; }
        public bool ForRepair { get; set; }
        public int? Rating { get; set; }
        public string? Particular { get; set; }
    }
}
