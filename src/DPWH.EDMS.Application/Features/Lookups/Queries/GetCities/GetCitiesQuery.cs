using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Lookups.Models;
using DPWH.EDMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetCities;

public record GetCitiesQuery(string ProvinceCode) : IRequest<AddressLookup>;

internal sealed class GetCitiesHandler : IRequestHandler<GetCitiesQuery, AddressLookup>
{
    private readonly IReadRepository _repository;

    public GetCitiesHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressLookup> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var provinceCode = request.ProvinceCode;
        var entity = await _repository
            .GeoLocationView
            .Where(d => d.Type == GeoLocationTypes.C.ToString() || d.Type == GeoLocationTypes.M.ToString())
            .Where(d => d.ParentId.Contains(provinceCode))
            .ToListAsync(cancellationToken);

        var citiesData = entity
            .OrderBy(data => data.Name)
            .Select(data => new SimpleKeyValueAddress
            (
                data.MyId,
                data.Name.ToString(),
                data.MyIdAdmin
            ))
            .ToList();

        return new AddressLookup("CitiesOrMunicipalities", citiesData);
    }
}