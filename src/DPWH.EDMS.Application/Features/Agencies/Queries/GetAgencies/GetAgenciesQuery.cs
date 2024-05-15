using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Agencies.Queries.GetAgencies;

public record GetAgenciesQuery : IRequest<IEnumerable<GetAgenciesResult>>;

internal sealed class GetAgenciesHandler : IRequestHandler<GetAgenciesQuery, IEnumerable<GetAgenciesResult>>
{
    private readonly IReadRepository _repository;

    public GetAgenciesHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetAgenciesResult>> Handle(GetAgenciesQuery request, CancellationToken cancellationToken)
    {
        var agencies = await _repository.AgenciesView.ToListAsync(cancellationToken);

        if (agencies.TrueForAll(a => a.AgencyCode == null))
        {
            throw new AppException("No agencies are properly configured");
        }

        return agencies
            .Where(a => a.AgencyCode is not null)
            .DistinctBy(a => a.AgencyCode)
            .Select(a => new GetAgenciesResult(a, agencies.Where(i => i.AgencyId == a.AgencyId)))
            .OrderBy(a => a.Code);
    }
}