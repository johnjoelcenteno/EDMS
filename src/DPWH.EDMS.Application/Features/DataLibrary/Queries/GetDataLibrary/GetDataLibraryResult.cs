namespace DPWH.EDMS.Application.Features.DataLibrary.Queries.GetDataLibrary;

public record GetDataLibraryResult
{
    public string Type { get; set; }
    public GetDataLibraryResultValue[] Data { get; set; }
}

public record GetDataLibraryResultValue
{
    public GetDataLibraryResultValue(Domain.Entities.DataLibrary entity)
    {
        Id = entity.Id;
        Value = entity.Value;
        IsDeleted = entity.IsDeleted;
        Created = entity.Created;
        CreatedBy = entity.CreatedBy;
    }

    public Guid Id { get; set; }
    public string Value { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? Created { get; set; }
    public string CreatedBy { get; set; }
}