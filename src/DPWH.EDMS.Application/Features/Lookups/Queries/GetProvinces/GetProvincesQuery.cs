using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Lookups.Models;
using DPWH.EDMS.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetProvinces;

public record GetProvincesQuery(string RegionCode) : IRequest<AddressLookup>;

internal sealed class GetProvincesHandler : IRequestHandler<GetProvincesQuery, AddressLookup>
{
    private readonly IReadRepository _repository;

    public GetProvincesHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressLookup> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
    {
        var regionCode = request.RegionCode;
        var entity = await _repository
            .GeolocationsView
            .Where(d => d.Type == GeoLocationTypes.P.ToString() && d.ParentId == regionCode)
            .ToListAsync(cancellationToken);

        var provinceData = entity
            .OrderBy(data => data.Name)
            .Select(data => new SimpleKeyValueAddress
            (
                data.MyId,
                data.Name.ToString(),
                data.MyIdAdmin
            ))
            .ToList();

        return new AddressLookup("Provinces", provinceData);
    }
}