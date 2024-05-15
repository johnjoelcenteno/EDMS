using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.RequestingOffices.Queries.GetRequestingOffices;

public record GetRequestingOfficeResult
{
    public GetRequestingOfficeResult(IReadOnlyList<RequestingOffice> requestingOffices)
    {
        var mainOffice = requestingOffices[0].Parent;

        RegionId = mainOffice.Id;
        RegionName = mainOffice.Name;

        ImplementingOffices = requestingOffices
            .Select(r => new GetRequestingOfficeResultItem(r.Id, r.Name))
            .OrderBy(r => r.SubOfficeName)
            .ToList();
        ImplementingOffices.Insert(0, new GetRequestingOfficeResultItem(mainOffice.Id, mainOffice.Name));
    }

    public string RegionId { get; set; }
    public string RegionName { get; set; }
    public List<GetRequestingOfficeResultItem> ImplementingOffices { get; set; }
}

public record GetRequestingOfficeResultItem(string SubOfficeId, string SubOfficeName);