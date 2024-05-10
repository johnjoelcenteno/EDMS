namespace DPWH.EDMS.Web.Shared.Models;

public class MenuModel
{
    public string? Text { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public bool Expanded { get; set; }
    public int Level { get; set; }
    public int SortOrder { get; set; }
    public List<string> AuthorizedRoles { get; set; } = new();
    public IEnumerable<MenuModel>? Children { get; set; }

}

public class FooterIconModel
{
    public string IconName { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public string IconCssClass { get; private set; } = string.Empty;
    public string Link { get; private set; } = string.Empty;   
}
