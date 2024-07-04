using DPWH.EDMS.Client.Shared.Models;

namespace DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;

public interface IExceptionHandlerService
{
    //Task Handle(ApiException<ProblemDetails> apiExtension, IToastService ToastService);
    Task HandleApiException(
        Func<Task> func,
        Action? afterSuccessCb = null,
        string successMessage = null);
    Task<T?> HandleApiException<T>(
        Func<Task<T?>> func,
        Action? afterSuccessCb = null,
        string? successMessage = null); 
}

