using DPWH.EDMS.Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using Telerik.FontIcons;

namespace DPWH.EDMS.Components;

public class RxBaseComponent : ComponentBase, IDisposable
{
    protected bool XSmall { get; set; }
    public List<BreadcrumbModel> BreadcrumbItems { get; set; } = new()
    {
         new() { Icon = FontIcon.Home, Url = "/"},
    };
    protected List<IDisposable> RxSubscriptions { get; set; } = new();
    public void Dispose() => RxSubscriptions.ForEach(sub => sub.Dispose());
}
