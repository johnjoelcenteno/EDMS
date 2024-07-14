using DPWH.EDMS.Domain.Common;
using UUIDNext;

namespace DPWH.EDMS.Domain.Entities;

public class DataLibrary : EntityBase
{
    private DataLibrary(string type, string value, string createdBy)
    {
        Id = Uuid.NewDatabaseFriendly(Database.SqlServer);
        Type = type;
        Value = value;
        SetCreated(createdBy);
    }

    public static DataLibrary Create(string type, string value, string createdBy)
    {
        return new DataLibrary(type, value, createdBy);
    }

    public void Update(string value, string updatedBy)
    {
        Value = value;
        SetModified(updatedBy);
    }

    public void Delete(string updatedBy)
    {
        if (IsDeleted)
        {
            return;
        }
        IsDeleted = true;
        SetModified(updatedBy);
    }

    public void Undelete(string updatedBy)
    {
        if (!IsDeleted)
        {
            return;
        }
        IsDeleted = false;
        SetModified(updatedBy);
    }

    public string Type { get; private set; }
    public string Value { get; private set; }
    public bool IsDeleted { get; private set; }
}