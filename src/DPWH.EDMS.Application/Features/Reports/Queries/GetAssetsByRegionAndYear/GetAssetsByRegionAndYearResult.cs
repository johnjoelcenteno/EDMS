using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByRegionAndYear;

public record GetAssetsByRegionAndYearResult
{
    public GetAssetsByRegionAndYearResult(string region, IReadOnlyList<Asset> assets)
    {
        Region = region;
        Assets = assets.Select(a => new GetAssetsByRegionAndYearResultItem(a)).ToArray();
    }

    public string Region { get; set; }
    public GetAssetsByRegionAndYearResultItem[] Assets { get; set; }
}

public record GetAssetsByRegionAndYearResultItem
{
    public GetAssetsByRegionAndYearResultItem(Asset asset)
    {
        var financialDetails = asset.FinancialDetails;

        ImplementingOffice = asset.ImplementingOffice ?? string.Empty;
        OfficialReceiptNumber = financialDetails.ORNumber ?? string.Empty;
        DateOfPayment = financialDetails.PaymentDate;
        AmountPaid = financialDetails.AmountPaid;
        PolicyNumber = financialDetails.PolicyNumber ?? string.Empty;
        PolicyId = financialDetails.PolicyID ?? string.Empty;
        EffectivityPeriod = financialDetails.EffectivityStart;
        Particular = financialDetails.Particular ?? string.Empty;
        InsuredValueBuilding = financialDetails.Building ?? "0";
        InsuredValueContent = financialDetails.Content ?? "0";
        Premium = financialDetails.Premium ?? decimal.Zero;
        TotalPremium = financialDetails.TotalPremium ?? decimal.Zero;
        Remarks = financialDetails.Remarks;
    }

    public string ImplementingOffice { get; set; }
    public string OfficialReceiptNumber { get; set; }
    public DateTimeOffset? DateOfPayment { get; set; }
    public decimal AmountPaid { get; set; }
    public string PolicyNumber { get; set; }
    public string PolicyId { get; set; }
    public DateTimeOffset? EffectivityPeriod { get; set; }
    public string Particular { get; set; }
    public string InsuredValueBuilding { get; set; }
    public string InsuredValueContent { get; set; }
    public decimal Premium { get; set; }
    public decimal TotalPremium { get; set; }
    public string? Remarks { get; set; }
}