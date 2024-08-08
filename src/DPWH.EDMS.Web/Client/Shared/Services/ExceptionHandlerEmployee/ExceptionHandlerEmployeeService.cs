using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerEmployee;

public class ExceptionHandlerEmployeeService : IExceptionHandlerEmployeeService
{

    private readonly IToastService _ToastService;

    public ExceptionHandlerEmployeeService(IToastService toastService)
    {
        _ToastService = toastService;
    }
    public async Task HandleApiException(
     Func<Task> func,
     Action? afterSuccessCb = null,
     string? successMessage = null,
     bool? isUserManagement = false,
     Func<bool, bool, Task>? EmployeeException = null)
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
            if (isUserManagement == true && EmployeeException != null)
            {
                if (apiEx.StatusCode == 400)
                {
                    await EmployeeException.Invoke(true, false);
                }
            }
            else
            {
                OnCatchError(apiEx);
            }
        }
        catch (Exception ex) when (ex is ApiException apiEx)
        {
            if (isUserManagement == true && EmployeeException != null)
            {
                if (apiEx.StatusCode == 404)
                {
                    await EmployeeException.Invoke(false, true);
                }
                else
                {
                    await EmployeeException.Invoke(true, false);
                }
            }
            else
            {
                OnCatchError(apiEx);
            }
        }
        catch (Exception ex)
        {
            _ToastService.ShowError(ex.Message);
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
