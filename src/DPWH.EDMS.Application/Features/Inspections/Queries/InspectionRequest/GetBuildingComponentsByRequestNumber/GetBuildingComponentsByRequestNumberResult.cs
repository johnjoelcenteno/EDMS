using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetBuildingComponentsByRequestNumber;

public record GetBuildingComponentsByRequestNumberResult
{
    public GetBuildingComponentsByRequestNumberResult(ProjectMonitoring? projectMonitoring, MaintenanceRequest? maintenanceRequest)
    {
        if (projectMonitoring != null)
        {
            BuildingComponents = new BuildingComponentsByRequestNumberModel
            {
                Components = projectMonitoring?.ProjectMonitoringBuildingComponents?
                    .GroupBy(x => x.Category)
                    .Select(group => new BuildingComponentsByRequestNumberCategoryModel
                    {
                        Category = group.Key,
                        Subcategory = group.Select(x => x.SubCategory).Distinct()
                    })
            };
        }
        if (maintenanceRequest != null)
        {
            BuildingComponents = new BuildingComponentsByRequestNumberModel
            {
                Components = maintenanceRequest?.MaintenanceRequestBuildingComponents?
                    .GroupBy(x => x.Category)
                    .Select(group => new BuildingComponentsByRequestNumberCategoryModel
                    {
                        Category = group.Key,
                        Subcategory = group.Select(x => x.SubCategory).Distinct()
                    })
            };
        }
    }

    public BuildingComponentsByRequestNumberModel? BuildingComponents { get; set; }
    public class BuildingComponentsByRequestNumberModel
    {
        public IEnumerable<BuildingComponentsByRequestNumberCategoryModel>? Components { get; set; }
    }
    public class BuildingComponentsByRequestNumberCategoryModel
    {
        public string? Category { get; set; }
        public IEnumerable<string>? Subcategory { get; set; }
    }
}
