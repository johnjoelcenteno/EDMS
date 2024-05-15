namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.AddDataLibrary;

public record AddDataLibraryResult
{
    public AddDataLibraryResult(Domain.Entities.DataLibrary entity)
    {
        Id = entity.Id;
        Type = entity.Type;
        Value = entity.Value;
    }

    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}