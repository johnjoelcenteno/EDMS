using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Users;

public interface IUsersService
{
    Task<GetUserByIdResultBaseApiResponse> GetById(Guid id);
    Task<DataSourceResult> Query(DataSourceRequest body);
    Task<CreateUserResultBaseApiResponse> Create(CreateUserCommand request);
    Task<UpdateUserResultBaseApiResponse> Update(Guid id, UpdateUserCommand request);
    Task<RemoveUserResultBaseApiResponse> Delete(Guid id);
    Task<CreateUserWithRoleResultBaseApiResponse> CreateUserWithRole(CreateUserWithRoleCommand body);
    Task<DeactivateUserResultBaseApiResponse> DeactivateUser(DeactivateUserCommand body);
    Task<GetUserByIdResultBaseApiResponse> GetUserByEmployeeId(string id);
    Task<UpdateResponse> UploadSignature(FileParameter document);
    Task<GetUserProfileDocumentModelBaseApiResponse> GetUserSignature();
    Task<GetUserProfileDocumentModelBaseApiResponse> GetUserSignatureByEmployeeId(string employeeId);

}