using FluentValidation;

namespace DPWH.EDMS.Application.Features.RequestingOffices.Commands.BatchCreateRequestingOffice;

public sealed class BatchCreateRequestingOfficeValidator : AbstractValidator<BatchCreateRequestingOfficeCommand>
{
    public BatchCreateRequestingOfficeValidator()
    {
        RuleFor(command => command.RequestingOffices)
            .NotEmpty()
            .WithMessage("RequestingOffices Envelope must not be null.")
            .ChildRules(v =>
            {
                v.RuleFor(e => e.Body)
                    .NotEmpty()
                    .WithMessage("RequestingOffices Body must not be null.")
                    .ChildRules(vb =>
                    {
                        vb.RuleFor(eb => eb.Response)
                            .NotEmpty()
                            .WithMessage("RequestingOffices Container must not be null.")
                            .ChildRules(vc =>
                            {
                                vc.RuleFor(ec => ec.Result)
                                    .NotEmpty()
                                    .WithMessage("RequestingOffices Container Result must not be null.")
                                    .ChildRules(vro =>
                                    {
                                        vro.RuleFor(evro => evro.Data)
                                            .NotEmpty()
                                            .WithMessage("ImplementingOffice collection must not be null.")
                                            .Must(d => d.Length > 0)
                                            .WithMessage("ImplementingOffices collection must not be empty.");
                                    });
                            });
                    });
            });
    }
}