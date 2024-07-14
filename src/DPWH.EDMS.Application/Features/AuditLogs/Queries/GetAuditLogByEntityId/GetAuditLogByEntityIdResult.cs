using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogByEntityId;

public record GetAuditLogByEntityIdResult
{
    public GetAuditLogByEntityIdResultDetail? LogDetails { get; set; }
    public IEnumerable<GetAuditLogByEntityIdResultItemChange>? Changes { get; set; }
}

public record GetAuditLogByEntityIdResultDetail
{
    public GetAuditLogByEntityIdResultDetail(ChangeLog entity)
    {        
        Model = new GetAuditLogByEntityIdResultItemModel(entity);
    }

    public GetAuditLogByEntityIdResultItemModel? Model { get; set; }
    public string? PropertyId { get; set; }
    public string? BuildingName { get; set; }
    public string? BuildingId { get; set; }
}

public record GetAuditLogByEntityIdResultItemModel
{
    public GetAuditLogByEntityIdResultItemModel(ChangeLog entity)
    {
        EntityId = entity.EntityId;
        Entity = entity.Entity;
        Action = entity.ActionType;
        Created = entity.ActionDate;
        CreatedBy = entity.UserName;
        EmployeeNumber = entity.EmployeeNumber;
    }

    public string? EntityId { get; set; }
    public string? Entity { get; set; }
    public string? Action { get; set; }
    public DateTimeOffset? Created { get; set; }
    public string? CreatedBy { get; set; }
    public string? EmployeeNumber { get; set; }
}

public record GetAuditLogByEntityIdResultItemChange(string? Field, string? From, string? To);