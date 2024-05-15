using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByPolicyNo;

public record GetAssetsByPolicyNoResult
{
    private const decimal DocumentaryStampPercentage = 1.125m;

    public GetAssetsByPolicyNoResult(string policyNumber, IReadOnlyList<Asset> entities)
    {
        var entity = entities[0];

        PolicyNumber = policyNumber;
        PolicyId = entity.FinancialDetails.PolicyNumber ?? string.Empty;
        Region = entity.Region ?? string.Empty;
        Assets = entities.Select(a => new GetAssetsByPolicyNoResultItem(a)).ToArray();
        TotalAppraisedValue = entities.Sum(a => a.AppraisedValue);
        TotalPremium = Assets.Sum(a => a.Premium) ?? decimal.Zero;
        //DocumentaryStamp = TotalPremium / DocumentaryStampPercentage;
        GrandTotal = TotalPremium;
    }

    public string PolicyNumber { get; set; }
    public string PolicyId { get; set; }
    public string Region { get; set; }
    public decimal TotalAppraisedValue { get; set; }
    public decimal TotalPremium { get; set; }
    public decimal DocumentaryStamp { get; set; }
    public decimal GrandTotal { get; set; }
    public GetAssetsByPolicyNoResultItem[] Assets { get; set; }
}

public record GetAssetsByPolicyNoResultItem
{
    public GetAssetsByPolicyNoResultItem(Asset entity)
    {
        OfficeName = entity.RequestingOffice ?? string.Empty;
        Location = $"{entity.CityOrMunicipality}, {entity.Province}";
        BuildingName = entity.Name ?? string.Empty;
        AppraisedValue = entity.AppraisedValue;
        Premium = entity.FinancialDetails.Premium;
        TotalPremium = entity.FinancialDetails.TotalPremium;
        EffectivityDate = entity.FinancialDetails.EffectivityStart;
        Remarks = entity.Remarks;
    }

    public string OfficeName { get; }
    public string Location { get; }
    public string BuildingName { get; }
    public decimal AppraisedValue { get; }
    public decimal? Premium { get; set; }
    public decimal? TotalPremium { get; set; }
    public DateTimeOffset? EffectivityDate { get; set; }
    public string? Remarks { get; }
}