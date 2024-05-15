namespace DPWH.EDMS.Application.Features.DataSync.Queries;
public record GetDataSyncQueryResult
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Result { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public string? CreatedBy { get; set; }
}