namespace DPWH.EDMS.Domain.Entities;

public class SystemLog
{
    private SystemLog() { }

    public static SystemLog Create(string? version, string? description, string createdBy, DateTimeOffset date)
    {
        var config = new SystemLog
        {
            Version = version,
            Description = description,
            Created = date,
            CreatedBy = createdBy
        };

        return config;
    }
    public Guid Id { get; set; }
    public string? Version { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }

    public void UpdateDetails(string? version, string? description, string createdBy, DateTimeOffset date)
    {
        Version = version;
        Description = description;
        CreatedBy = createdBy;
        Created = date;
    }
}
