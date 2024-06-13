using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Web.Client.Shared.Validators;
using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Services.Core;

public static class FluentValidatorServiceProvider
{
    public static IServiceCollection AddFluentValidatorService(this IServiceCollection services)
    {
        // Validators
        services.AddTransient<IValidator<DocumentRequestModel>, DocumentRequestModelValidator>();

        return services;
    }
}
