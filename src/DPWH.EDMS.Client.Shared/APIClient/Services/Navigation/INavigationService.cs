using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Navigation
{
    public interface INavigationService
    {
        Task<CreateResponse> Create(CreateMenuItemModel request);
        Task<DataSourceResult> Query(DataSourceRequest body);
    }
}