using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Lookups.Models;
using DPWH.EDMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetRegions;

public record GetRegionsQuery : IRequest<AddressLookup>;

internal sealed class GetRegionsHandler : IRequestHandler<GetRegionsQuery, AddressLookup>
{
    private readonly IReadRepository _repository;

    public GetRegionsHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressLookup> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository
            .GeolocationsView
            .Where(d => d.Type == GeoLocationTypes.R.ToString())
            .ToListAsync(cancellationToken);

        var regionData = entity
            .OrderBy(data => data.Name)
            .Select(data => new SimpleKeyValueAddress
            (
                data.MyId,
                data.Name.ToString(),
                data.MyIdAdmin
            ))
            .ToList();

        return new AddressLookup("Regions", regionData);
    }
}