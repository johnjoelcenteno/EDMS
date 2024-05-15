using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RequestingOffices.Queries.GetRequestingOffices;

public record GetRequestingOfficesQuery : IRequest<IEnumerable<GetRequestingOfficeResult>>;

internal sealed class GetRequestingOfficeHandler : IRequestHandler<GetRequestingOfficesQuery, IEnumerable<GetRequestingOfficeResult>>
{
    private readonly IReadRepository _repository;

    public GetRequestingOfficeHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetRequestingOfficeResult>> Handle(GetRequestingOfficesQuery request, CancellationToken cancellationToken)
    {
        var requestingOffices = await _repository.RequestingOfficesView
            .Include(r => r.Parent)
            .Where(r => r.ParentId != null)
            .GroupBy(r => r.ParentId)
            .ToListAsync(cancellationToken);

        if (!requestingOffices.Any())
        {
            throw new AppException("Requesting Office table is empty, please run data sync");
        }

        return requestingOffices.Select(g => new GetRequestingOfficeResult(g.ToArray()));
    }
}