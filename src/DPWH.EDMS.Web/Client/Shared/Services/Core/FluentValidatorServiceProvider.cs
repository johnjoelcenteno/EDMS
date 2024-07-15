using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;
using DPWH.EDMS.Web.Client.Shared.Validators;
using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Services.Core;

public static class FluentValidatorServiceProvider
{
    public static IServiceCollection AddFluentValidatorService(this IServiceCollection services)
    {
        // Validators
        services.AddTransient<IValidator<CreateRecordRequest>, CreateDocumentRequestModelValidator>();
        services.AddTransient<IValidator<ConfigModel>,AddEditDataLibraryRequestFormValidator>();
        services.AddTransient<IValidator<RecordsLibraryModel>,AddEditRecordTypeFormValidator>();

        return services;
    }
}
