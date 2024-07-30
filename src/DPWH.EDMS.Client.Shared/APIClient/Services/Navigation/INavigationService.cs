using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;

public interface INavigationService
{
    Task<CreateResponse> Create(CreateMenuItemModel request);
    Task<DataSourceResult> Query(DataSourceRequest body);
    Task<DataSourceResult> QueryByNavType(string navType, DataSourceRequest body);
    Task<DeleteResponse> Delete(Guid id);
    Task<MenuItemModelBaseApiResponse> GetById(Guid id);
    Task<GuidNullableBaseApiResponse> Update(Guid id, UpdateMenuItemModel body);
}