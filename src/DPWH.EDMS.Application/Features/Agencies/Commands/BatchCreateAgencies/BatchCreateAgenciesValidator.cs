using FluentValidation;

namespace DPWH.EDMS.Application.Features.Agencies.Commands.BatchCreateAgencies;

public sealed class BatchCreateAgenciesValidator : AbstractValidator<BatchCreateAgenciesCommand>
{
    public BatchCreateAgenciesValidator()
    {
        RuleFor(command => command.NationalGovtAgencies)
            .NotEmpty()
            .WithMessage("NationalGovtAgencies Envelope must not be null.")
            .ChildRules(v =>
            {
                v.RuleFor(e => e.Body)
                    .NotEmpty()
                    .WithMessage("NationalGovtAgencies Body must not be null.")
                    .ChildRules(vb =>
                    {
                        vb.RuleFor(eb => eb.Response)
                            .NotEmpty()
                            .WithMessage("NationalGovtAgencies Container must not be null.")
                            .ChildRules(vc =>
                            {
                                vc.RuleFor(ec => ec.Result)
                                    .NotEmpty()
                                    .WithMessage("NationalGovtAgencies Container Result must not be null.")
                                    .ChildRules(va =>
                                    {
                                        va.RuleFor(eva => eva.Data)
                                            .NotEmpty()
                                            .WithMessage("NationalGovtAgency collection must not be null.")
                                            .Must(d => d.Length > 0)
                                            .WithMessage("NationalGovtAgency collection must not be empty.");
                                    });
                            });
                    });
            });
    }
}