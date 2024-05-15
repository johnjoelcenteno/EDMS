using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class GeoLocation : EntityBase
{
    private GeoLocation(string myId, string myIdAdmin, string name, string type, string parentId)
    {
        Id = Guid.NewGuid();
        MyId = myId;
        MyIdAdmin = myIdAdmin;
        Name = name;
        Type = type;
        ParentId = parentId;
    }

    public static GeoLocation Create(string myId, string myIdAdmin, string name, string type, string parentId, string createdBy)
    {
        var location = new GeoLocation(myId, myIdAdmin, name, type, parentId);
        location.SetCreated(createdBy);

        return location;
    }

    public void UpdateDetails(string myId, string myIdAdmin, string name, string type, string parentId, string modifiedBy)
    {
        MyId = myId;
        MyIdAdmin = myIdAdmin;
        Name = name;
        Type = type;
        ParentId = parentId;

        SetModified(modifiedBy);
    }

    public string MyId { get; set; }
    public string MyIdAdmin { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string ParentId { get; set; }
    public Guid? ParentRef { get; set; }
}
