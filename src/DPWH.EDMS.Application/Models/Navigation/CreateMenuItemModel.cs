using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Models.Navigation;

//public class MenuItemModel
//{
//    public Guid Id { get; set; }
//    public string? Text { get; set; }
//    public string? Url { get; set; }
//    public string? Icon { get; set; }
//    public bool Expanded { get; set; }
//    public int Level { get; set; }
//    public int SortOrder { get; set; }
//    public List<string> AuthorizedRoles { get; set; } = new();
//    public IEnumerable<MenuItemModel>? Children { get; set; }
//}

public record class CreateMenuItemModel(
    string Text,
    string? Url,
    string? Icon,
    bool Expanded,
    int Level,
    int SortOrder,
    List<string> AuthorizedRoles,
    Guid? ParentId
);
