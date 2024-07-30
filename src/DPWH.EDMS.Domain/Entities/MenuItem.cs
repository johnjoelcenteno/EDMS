using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class MenuItem : EntityBase
{
    private MenuItem() { }

    private MenuItem(Guid id, string? text, string? url, string? icon, bool expanded, int level, int sortOrder, string navType, List<string> authorizedRoles, Guid? parentId)
    {
        Id = id;
        Text = text;
        Url = url;
        Icon = icon;
        Expanded = expanded;
        Level = level;
        SortOrder = sortOrder;
        NavType = navType;
        AuthorizedRoles = authorizedRoles;
        ParentId = parentId;
    }

    public static MenuItem Create(string? text, string? url, string? icon, bool expanded, int level, int sortOrder, string navType, List<string> authorizedRoles, Guid? parentId, string createdBy)
    {
        var id = Guid.NewGuid();

        var menuItem = new MenuItem(id, text, url, icon, expanded, level, sortOrder, navType, authorizedRoles, parentId);
        menuItem.SetCreated(createdBy);
        return menuItem;
    }

    public void Update(string? text, string? url, string? icon, bool expanded, int level, int sortOrder, string navType, List<string> authorizedRoles, Guid? parentId, string modifiedBy)
    {
        Text = text;
        Url = url;
        Icon = icon;
        Expanded = expanded;
        Level = level;
        SortOrder = sortOrder;
        NavType = navType;
        AuthorizedRoles = authorizedRoles;
        ParentId = parentId;
        SetModified(modifiedBy);
    }

    public Guid Id { get; set; }
    public string? Text { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public bool Expanded { get; set; }
    public int Level { get; set; }
    public int SortOrder { get; set; }
    public string NavType { get; set; }
    public List<string> AuthorizedRoles { get; set; } = new();
    public Guid? ParentId { get; set; }
}
