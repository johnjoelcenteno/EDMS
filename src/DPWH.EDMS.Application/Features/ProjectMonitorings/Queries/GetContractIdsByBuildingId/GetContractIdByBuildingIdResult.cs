using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetContractIdsByBuildingId;

public record GetContractIdByBuildingIdResult
{
    public GetContractIdByBuildingIdResult(List<ProjectMonitoring> entity)
    {
        ProjectMonitoringContracts = entity.Select(x => new ProjectMonitoringContract
        {
            ContractId = x.ContractId,
            Status = x.Status,
            ProjectName = x.ProjectName
        }).ToList();
    }

    public List<ProjectMonitoringContract> ProjectMonitoringContracts { get; set; }

    public class ProjectMonitoringContract
    {
        public string ContractId { get; set; }
        public string Status { get; set; }
        public string ProjectName { get; set; }
    }
}
