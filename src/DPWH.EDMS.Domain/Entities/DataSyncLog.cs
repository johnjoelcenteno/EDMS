using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Domain.Entities;

public class DataSyncLog
{
    public DataSyncLog()
    {
    }

    public static DataSyncLog Create(string type, string result, string? description, string createdBy)
    {
        return new DataSyncLog
        {
            Type = type,
            Result = result,
            Description = description,
            CreatedBy = createdBy,
            Created = DateTimeOffset.Now
        };
    }

    [Key]
    public int Id { get; set; }
    public string Type { get; set; }
    public string Result { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
}
