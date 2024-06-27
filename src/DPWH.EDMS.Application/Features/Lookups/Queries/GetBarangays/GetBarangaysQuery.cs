using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Lookups.Models;
using DPWH.EDMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetBarangays;

public record GetBarangaysQuery(string CityCode) : IRequest<AddressLookup>;

internal sealed class GetBarangaysHandler : IRequestHandler<GetBarangaysQuery, AddressLookup>
{
    private readonly IReadRepository _repository;

    public GetBarangaysHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressLookup> Handle(GetBarangaysQuery request, CancellationToken cancellationToken)
    {
        var cityOrMunicipalityCode = request.CityCode;

        var entity = await _repository
            .GeolocationsView
            .Where(d => d.Type == GeoLocationTypes.B.ToString() && d.ParentId.Contains(cityOrMunicipalityCode))
            .ToListAsync(cancellationToken);

        var barangayData = entity
            .OrderBy(data => data.Name)
            .Select(data => new SimpleKeyValueAddress
            (
                data.MyId,
                data.Name.ToString(),
                data.MyIdAdmin
            ))
            .ToList();

        return new AddressLookup("Barangays", barangayData);
    }
}