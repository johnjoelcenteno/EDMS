using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Shared;

public class RxBaseComponent : ComponentBase, IDisposable
{
    protected List<IDisposable> RxSubscriptions { get; set; } = new();
    public void Dispose() => RxSubscriptions.ForEach(sub => sub.Dispose());
}
