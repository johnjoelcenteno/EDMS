using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using Humanizer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Addresses.Commands.SyncAddress;

public record SyncAddressCommand(GeoRegionResponse GeoRegionResponse) : IRequest<int>;

internal sealed class SyncAddressHandler(ClaimsPrincipal principal, IWriteRepository repository) : IRequestHandler<SyncAddressCommand, int>
{
    private const string RomanNumeralPattern = " (XC|XL|L?X{0,3})(IX|IV|V?I{0,3})(-|$)";
    private readonly Regex _regex = new(RomanNumeralPattern, RegexOptions.IgnoreCase);

    public async Task<int> Handle(SyncAddressCommand request, CancellationToken cancellationToken)
    {
        await repository.Database.ExecuteSqlRawAsync("DELETE FROM Geolocation", cancellationToken);

        var geoLocations = request.GeoRegionResponse.GetLocations()
            .Select(g => GeoLocation.Create(g.MyId!, g.MyIdAdmin!, ApplyProperCasing(g.Name), g.Type!, g.ParentId!, principal.GetUserName()))
            .ToList();

        geoLocations.ForEach(g => g.ParentRef = geoLocations.FirstOrDefault(l => l.MyId == g.ParentId)?.Id);

        var recordCount = geoLocations.Count;
        while (geoLocations.Count > 0)
        {
            geoLocations = await TakeoutParents(geoLocations, cancellationToken);
        }

        return recordCount;
    }

    private string ApplyProperCasing(string? name)
    {
        if (name is null)
        {
            return string.Empty;
        }

        var decodedName = HttpUtility.HtmlDecode(name);
        var titleCased = decodedName.ToLowerInvariant().Transform(To.TitleCase);

        return _regex.Replace(titleCased, m => m.Value.ToUpper());
    }

    private async Task<List<GeoLocation>> TakeoutParents(IReadOnlyList<GeoLocation> list, CancellationToken cancellationToken)
    {
        var parents = list
            .Where(l => l.ParentRef is null || list.All(i => i.Id != l.ParentRef))
            .ToList();

        await repository.Geolocations.AddRangeAsync(parents, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return list.Except(parents).ToList();
    }
}