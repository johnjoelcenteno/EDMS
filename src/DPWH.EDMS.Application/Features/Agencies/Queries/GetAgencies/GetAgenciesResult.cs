using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Agencies.Queries.GetAgencies;

public class GetAgenciesResult
{
    public GetAgenciesResult(Agency parentAgency, IEnumerable<Agency> attachedAgencies)
    {
        Id = parentAgency.AgencyNumberCode;
        Code = parentAgency.AgencyCode;
        Name = parentAgency.AgencyName;
        AttachedAgencies = attachedAgencies
            .Select(a => a.AttachedAgencyName)
            .Order()
            .ToArray();
    }

    public string? Id { get; }
    public string? Code { get; }
    public string Name { get; }
    public string[] AttachedAgencies { get; }
}