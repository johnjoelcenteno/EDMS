using FluentValidation;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByPolicyNo;

public sealed class GetAssetsByPolicyNoValidator : AbstractValidator<GetAssetsByPolicyNoQuery>
{
    public GetAssetsByPolicyNoValidator()
    {
        RuleFor(query => query.PolicyNumber)
            .NotEmpty()
            .WithMessage("Policy No. should not be empty or null.");
    }
}