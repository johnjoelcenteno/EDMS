namespace DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerPIS;

public interface IExceptionPISHandlerService
{
    Task HandleApiException(
        Func<Task> func,
        Action? afterSuccessCb = null,
        string successMessage = null,
        bool? isUserManagement = false,
        Func<bool, bool, Task>? PISException = null);
}
