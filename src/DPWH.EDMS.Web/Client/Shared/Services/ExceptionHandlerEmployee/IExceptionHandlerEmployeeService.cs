namespace DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerEmployee;

public interface IExceptionHandlerEmployeeService
{
    Task HandleApiException(
       Func<Task> func,
       Action? afterSuccessCb = null,
       string successMessage = null,
       bool? isUserManagement = false,
       Func<bool, bool, Task>? EmployeeException = null);
}
