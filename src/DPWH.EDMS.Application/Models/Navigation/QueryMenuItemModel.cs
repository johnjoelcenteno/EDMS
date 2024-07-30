using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Models.Navigation;

public class QueryMenuItemModel
{
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
