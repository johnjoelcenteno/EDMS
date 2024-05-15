using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class RequestingOffice : EntityBase
{
    private RequestingOffice() { }

    private RequestingOffice(
        string id,
        string name,
        string? parentId,
        string? numberCode,
        string createdBy)
    {
        Id = id;
        Name = name;
        ParentId = parentId;
        NumberCode = numberCode;

        SetCreated(createdBy);
    }

    public static RequestingOffice Create(
        string id,
        string name,
        string? parentId,
        string? numberCode,
        string createdBy)
    {
        parentId = CheckSelfReferentialParent(id, parentId);
        return new RequestingOffice(id, name, parentId, numberCode, createdBy);
    }

    public void Update(RequestingOffice requestingOffice, string modifiedBy)
    {
        Name = requestingOffice.Name;
        ParentId = CheckSelfReferentialParent(Id, requestingOffice.ParentId);
        NumberCode = requestingOffice.NumberCode;

        SetModified(modifiedBy);
    }

    private static string? CheckSelfReferentialParent(string id, string? parentId)
    {
        return id != parentId ? parentId : null;
    }

    public new string Id { get; private set; }
    public string Name { get; private set; }
    public string? ParentId { get; private set; }
    public string? NumberCode { get; private set; }
    public RequestingOffice? Parent { get; set; }
}