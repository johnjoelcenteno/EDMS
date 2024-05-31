
using Telerik.FontIcons;

namespace DPWH.EDMS.Client.Shared.Models;

public class BreadcrumbModel
{
    public string Text { get; set; }
    public string? Icon { get; set; } = "menu";
    public string Url { get; set; }
}
