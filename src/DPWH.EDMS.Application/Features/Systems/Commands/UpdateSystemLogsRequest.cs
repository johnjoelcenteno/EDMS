namespace DPWH.EDMS.Application.Features.Systems.Commands;

public class UpdateSystemLogsRequest
{
    public string? Version { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }

}
