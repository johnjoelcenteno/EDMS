using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;

public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly IToastService _ToastService;

    public ExceptionHandlerService(IToastService toastService)
    {
        _ToastService = toastService;
    }

    public async Task HandleApiException(
        Func<Task> func,
        Action? afterSuccessCb = null,
        string? successMessage = null)
    {
        try
        {
            await func.Invoke();

            if (!string.IsNullOrEmpty(successMessage))
            {
                _ToastService.ShowSuccess(successMessage);
                if (afterSuccessCb != null)
                {
                    afterSuccessCb.Invoke();
                }
            }

        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiEx)
        {
            OnCatchError(apiEx);
        }
        catch (Exception ex) when (ex is ApiException apiEx)
        {
            OnCatchError(apiEx);
        }
        catch (Exception ex)
        {
            _ToastService.ShowError(ex.Message);
        }
    }

    public async Task<T?> HandleApiException<T>(Func<Task<T?>> func, Action? afterSuccessCb, string? successMessage = null)
    {
        try
        {
            T result = await func.Invoke();

            if (afterSuccessCb != null)
            {
                if (!string.IsNullOrEmpty(successMessage))
                {
                    _ToastService.ShowSuccess(successMessage);
                }

                afterSuccessCb.Invoke();
            }
            if (result == null)
            {
                _ToastService.ShowError("Object reference not set to an instance of an object");
            }
            return result;
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiEx)
        {
            OnCatchError(apiEx);
            throw;
        }
        catch (Exception ex) when (ex is ApiException apiEx)
        {
            OnCatchError(apiEx);
            throw;
        }
        catch (Exception ex)
        {
            _ToastService.ShowError(ex.Message);
            throw;
        }
    }

    private void OnCatchError(ApiException apiEx)
    {
        var htmlContent = new RenderFragment(builder =>
        {
            builder.AddMarkupContent(0, apiEx.Message);
        });
        _ToastService.ShowError(htmlContent);

    }
    private void OnCatchError(ApiException<ProblemDetails> apiEx)
    {
        var problemDetails = apiEx.Result;
        if (problemDetails.AdditionalProperties.ContainsKey("error"))
        {
            var error = problemDetails.AdditionalProperties["error"].ToString();
            _ToastService.ShowError(error ?? string.Empty);
        }

        else if (problemDetails.AdditionalProperties.ContainsKey("errors"))
        {
            var errors = problemDetails.AdditionalProperties["errors"].ToString();
            List<Dictionary<string, object>> errorList =
                JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(errors ?? string.Empty)!;
            if (errorList != null && errorList.Count > 0)
            {
                string list = "";

                foreach (var error in errorList)
                {
                    var errorMessage = error["errorMessage"].ToString();
                    list += $@"<li>{errorMessage}</li>";
                }

                var htmlDisplay = new MarkupString($@"<ul>{list}</ul>").ToString();

                var htmlContent = new RenderFragment(builder =>
                {
                    builder.AddMarkupContent(0, htmlDisplay);
                });

                _ToastService.ShowError(htmlContent);
            }
        }
    }
}
