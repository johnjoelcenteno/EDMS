using System.Reactive.Subjects;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;

public class NavRx
{
    public BehaviorSubject<bool> IsExpanded = new BehaviorSubject<bool>(true);
}
