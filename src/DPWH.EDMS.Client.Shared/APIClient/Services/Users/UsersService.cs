using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;

public class UsersService : IUsersService
{
    private readonly UsersClient _client;

    public UsersService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.WebServerClientName);
        _client = new UsersClient(httpClient);
    }

    public async Task<GetUserByIdResultBaseApiResponse> GetById(Guid id)
    {
        return await _client.GetUserByIdAsync(id);
    }

    public async Task<DataSourceResult> Query(DataSourceRequest body)
    {
        return await _client.QueryUsersAsync(body);
    }

    public async Task<CreateUserResultBaseApiResponse> Create(CreateUserCommand request)
    {
        return await _client.CreateUserAsync(request);
    }

    public async Task<UpdateUserResultBaseApiResponse> Update(Guid id, UpdateUserCommand request)
    {
        return await _client.UpdateUserAsync(id, request);
    }

    public async Task<RemoveUserResultBaseApiResponse> Delete(Guid id)
    {
        return await _client.DeleteUserAsync(id);
    }

    public async Task<CreateUserWithRoleResultBaseApiResponse> CreateUserWithRole(CreateUserWithRoleCommand body)
    {
        return await _client.CreateUserWithRoleAsync(body);
    }

    public async Task<DeactivateUserResultBaseApiResponse> DeactivateUser(DeactivateUserCommand body)
    {
        return await _client.DeactivateUserAsync(body);
    }
    public async Task<GetUserByIdResultBaseApiResponse> GetUserByEmployeeId(string id)
    {
        return await _client.GetUserByEmployeeIdAsync(id);
    }
    public Task<UpdateResponse> UploadSignature(FileParameter document)
    {
        return _client.UploadSignatureAsync(document);
    }
    public Task<GetUserProfileDocumentModelBaseApiResponse> GetUserSignature()
    {
        return _client.GetUserSignatureAsync();
    }
}