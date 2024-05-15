namespace DPWH.EDMS.Domain.Entities;

public class ChangeLogItem
{
    private ChangeLogItem(string? field, string? from, string? to)
    {
        Field = field;
        From = from;
        To = to;
    }

    public static ChangeLogItem Create(string? field, string? from, string? to)
    {
        return new ChangeLogItem(field, from, to);
    }

    public int Id { get; set; }
    public string? Field { get; private set; }
    public string? From { get; private set; }
    public string? To { get; private set; }

    public int ChangeLogId { get; set; }
    public ChangeLog ChangeLog { get; set; }
}