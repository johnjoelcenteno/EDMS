namespace DPWH.EDMS.Domain.Entities;

public class ChangeLog
{
    private ChangeLog(string entityId, string entity, string? propertyName, string? controlNumber, string actionType, string? userId, string? userName, string? firstName, string? lastName,
        string? middleInitial, string? employeeNumber)
    {
        EntityId = entityId;
        Entity = entity;
        PropertyName = propertyName;
        ControlNumber = controlNumber;
        ActionType = actionType;
        UserId = userId;
        UserName = userName;
        FirstName = firstName;
        MiddleInitial = middleInitial;
        LastName = lastName;
        EmployeeNumber = employeeNumber;
        ActionDate = DateTimeOffset.Now;
         
    }

    public static ChangeLog Create(string entityId, string entity, string? propertyName, string? controlNumber, string actionType, string? userId, string? userName, string? firstName,
        string? lastName, string middleInitial, string? employeeNumber)
    {
        return new ChangeLog(entityId, entity, propertyName, controlNumber, actionType, userId, userName, firstName, lastName, middleInitial, employeeNumber);
    }

    public static ChangeLog Create(string entityId, string entity, string? propertyName, string? controlNumber,  string actionType, string? userId, string? userName, string? firstName, 
        string? lastName, string? middleInitial, string? employeeNumber, DateTimeOffset created, IEnumerable<ChangeLogItem> changes)
    {
        var changeLog = new ChangeLog(entityId, entity, propertyName, controlNumber, actionType, userId, userName, firstName,
            lastName, middleInitial, employeeNumber);
        changeLog.ActionDate = created;
        changeLog.Changes = changes;

        return changeLog;
    }

    public static ChangeLog Create(string entityId, string entity, string? propertyName, string? controlNumber, string actionType, string? userId, string? userName, string? firstName,
        string? lastName, string? middleInitial, string? employeeNumber, IEnumerable<ChangeLogItem> changes)
    {
        var changeLog = new ChangeLog(entityId, entity, propertyName, controlNumber, actionType, userId, userName, firstName,
            lastName, middleInitial, employeeNumber);
        changeLog.Changes = changes;
        changeLog.ActionDate = DateTimeOffset.Now;

        return changeLog;
    }

    public int Id { get; set; }
    public string EntityId { get; private set; }
    public string Entity { get; private set; }
    public string? PropertyName { get; private set; }
    public string? ControlNumber { get; private set; }
    public string ActionType { get; private set; }
    public string? UserId { get; private set; }
    public string? UserName { get; private set; }
    public string? FirstName { get; private set; }
    public string? MiddleInitial { get; private set; }
    public string? LastName { get; private set; }
    public string? EmployeeNumber { get; private set; }
    public DateTimeOffset ActionDate { get; private set; }
     
    public IEnumerable<ChangeLogItem> Changes { get; private set; }
}