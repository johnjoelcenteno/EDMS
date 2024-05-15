namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.DeleteDataLibrary;

public record DeleteDataLibraryResult
{
    public DeleteDataLibraryResult(Domain.Entities.DataLibrary entity)
    {
        Id = entity.Id;
        Type = entity.Type;
        Value = entity.Value;
        IsDeleted = entity.IsDeleted;
    }

    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public bool IsDeleted { get; set; }
}