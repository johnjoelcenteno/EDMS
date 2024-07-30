using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Models.Navigation;

public record class UpdateMenuItemModel(
    string Text,
    string? Url,
    string? Icon,
    bool Expanded,
    int Level,
    int SortOrder,
    string NavType,
    List<string> AuthorizedRoles,
    Guid? ParentId
);
