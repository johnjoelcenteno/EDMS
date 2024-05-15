using FluentValidation;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.CreateMaintenanceRequestDocument;

public class MaintenanceDocumentValidator : AbstractValidator<CreateMaintenanceRequestDocumentRequest>
{
    public MaintenanceDocumentValidator()
    {
        RuleFor(param => param.MaintenanceRequestId).NotEmpty();
        RuleFor(param => param.File).NotNull();
    }
}
