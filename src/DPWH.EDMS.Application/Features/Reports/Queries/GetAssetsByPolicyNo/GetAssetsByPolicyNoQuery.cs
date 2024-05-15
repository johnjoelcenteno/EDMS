using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByPolicyNo;

public record GetAssetsByPolicyNoQuery(string PolicyNumber) : IRequest<GetAssetsByPolicyNoResult>;

internal sealed class GetAssetsByPolicyNoHandler : IRequestHandler<GetAssetsByPolicyNoQuery, GetAssetsByPolicyNoResult>
{
    private readonly IReadRepository _repository;

    public GetAssetsByPolicyNoHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAssetsByPolicyNoResult> Handle(GetAssetsByPolicyNoQuery request, CancellationToken cancellationToken)
    {
        var assets = await _repository.AssetsView
            .Include(a => a.FinancialDetails)
            .Where(a => a.FinancialDetails.PolicyNumber == request.PolicyNumber)
            .ToListAsync(cancellationToken);

        if (!assets.Any())
        {
            return new GetAssetsByPolicyNoResult(request.PolicyNumber, Array.Empty<Asset>());
        }

        return new GetAssetsByPolicyNoResult(request.PolicyNumber, assets);
    }
}