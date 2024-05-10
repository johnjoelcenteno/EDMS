using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Services.Core;

public static class FluentValidatorServiceProvider
{
    public static IServiceCollection AddFluentValidatorService(this IServiceCollection services)
    {
        // Validators
        //services.AddTransient<IValidator<CustomerAddressModel>, CustomerAddressValidator>();

        return services;
    }
}
