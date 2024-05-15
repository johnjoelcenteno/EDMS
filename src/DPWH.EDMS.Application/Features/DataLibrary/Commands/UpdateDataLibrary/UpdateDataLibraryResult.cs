namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.UpdateDataLibrary;

public record UpdateDataLibraryResult
{
    public UpdateDataLibraryResult(Domain.Entities.DataLibrary entity, string oldValue)
    {
        Id = entity.Id;
        Type = Enum.Parse<DataLibraryTypes>(entity.Type);
        OldValue = oldValue;
        Value = entity.Value;
    }

    public Guid Id { get; set; }
    public DataLibraryTypes Type { get; set; }
    public string OldValue { get; set; }
    public string Value { get; set; }
}