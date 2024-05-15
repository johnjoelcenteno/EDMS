namespace DPWH.EDMS.Application.Features.Systems.Queries.Models;

public class SystemLogsModel
{
    public Guid Id { get; set; }
    public string Version { get; set; }
    public string Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public string? CreatedBy { get; set; }
}
