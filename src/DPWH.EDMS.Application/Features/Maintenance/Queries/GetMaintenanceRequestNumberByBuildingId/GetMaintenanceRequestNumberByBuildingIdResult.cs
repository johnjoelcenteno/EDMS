using DPWH.EDMS.Domain.Entities;


namespace DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestNumberByBuildingId;

public record GetMaintenanceRequestNumberByBuildingIdResult
{
    public GetMaintenanceRequestNumberByBuildingIdResult(List<MaintenanceRequest> entity)
    {
        MaintenanceRequestNumber = entity.Select(x => new MaintenanceRequestNumbers
        {
            RequestNumbers = x.RequestNumber,
            Status = x.Status,
            ProposedProjectName = x.PurposeProjectName
        }).ToList();
    }

    public List<MaintenanceRequestNumbers> MaintenanceRequestNumber { get; set; }

    public class MaintenanceRequestNumbers
    {
        public string RequestNumbers { get; set; }
        public string Status { get; set; }
        public string ProposedProjectName { get; set; }
    }
}

