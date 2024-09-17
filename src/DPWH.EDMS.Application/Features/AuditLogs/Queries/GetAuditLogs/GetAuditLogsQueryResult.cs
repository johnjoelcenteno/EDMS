using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Entities;

namespace DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogs;
public record GetAuditLogsQueryResult
{
    public GetAuditLogsQueryResult(ChangeLog entity, IEnumerable<ApplicationUser>? targetUsers)
    {
        EntityId = entity.EntityId;
        Entity = entity.Entity;     
        PropertyName = entity.PropertyName;
        Action = entity.ActionType;
        CreatedBy = entity.UserName;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
        MiddleInitial = entity.MiddleInitial;
        EmployeeNumber = entity.EmployeeNumber;
        Created = entity.ActionDate;

        var targetUser = targetUsers?.FirstOrDefault(user => user.Id == entity.EntityId)?.UserBasicInfo;
        TargetUser = string.IsNullOrWhiteSpace(targetUser?.LastName) && string.IsNullOrWhiteSpace(targetUser?.FirstName) && string.IsNullOrWhiteSpace(targetUser?.MiddleInitial)
            ? null
            : $"{targetUser.LastName}, {targetUser.FirstName} {targetUser.MiddleInitial}";
        ControlNumber = entity.ControlNumber;
        Changes = entity.Changes
                        .Where(i => i.Field != "LastModified")
                        .Select(i => new GetAuditLogsQueryResultItemChange(i.Field, i.From, i.To))
                        .ToList();
    }

    public string? EntityId { get; set; }
    public string? Entity { get; set; }
    public string? PropertyId { get; private set; }
    public string? BuildingId { get; private set; }
    public string? PropertyName { get; private set; }
    public string? Action { get; set; }
    public string? CreatedBy { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleInitial { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? TargetUser { get; set; }
    public DateTimeOffset? Created { get; set; }
    public string? ControlNumber { get; set; }
    public IList<GetAuditLogsQueryResultItemChange> Changes { get; set; }
}

public record GetAuditLogsQueryResultItemChange(string? Field, string? From, string? To);