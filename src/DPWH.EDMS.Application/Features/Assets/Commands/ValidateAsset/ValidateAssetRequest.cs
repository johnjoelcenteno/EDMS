using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Application.Features.Assets.Commands.ValidateAsset;

public class ValidateAssetRequest
{
    public Guid Id { get; set; }
    public string? PropertyId { get; set; }
    public string? BuildingId { get; set; }
    public string? Name { get; set; }
    public string? PropertyStatus { get; set; }
    public string? RequestingOffice { get; set; }
    public string? ImplementingOffice { get; set; }
    public string? Agency { get; set; }
    public string? AttachedAgency { get; set; }
    public string? Group { get; set; }
    public string? Region { get; set; }
    public string RegionId { get; set; }
    public string Province { get; set; }
    public string ProvinceId { get; set; }
    public string CityOrMunicipality { get; set; }
    public string CityOrMunicipalityId { get; set; }
    public string Barangay { get; set; }
    public string BarangayId { get; set; }
    public string ZipCode { get; set; }
    public string StreetAddress { get; set; }
    public decimal? LotArea { get; set; }
    public decimal? FloorArea { get; set; }
    public int Floors { get; set; }
    public int YearConstruction { get; set; }
    public int? MonthConstruction { get; set; }
    public int? DayConstruction { get; set; }
    public string? ConstructionType { get; set; }
    public string? LotStatus { get; set; }
    public string? BuildingStatus { get; set; }
    public decimal ZonalValue { get; set; }
    public decimal BookValue { get; set; }
    public decimal AppraisedValue { get; set; }
    public ValidateFinancialDetailsRequest? FinancialDetails { get; set; }
}

public class ValidateAssetStatusRequest
{
    public AssetStatus Status { get; }
    public string? Reason { get; set; }
}

public class ValidateFinancialDetailsRequest
{
    public DateTimeOffset DateAcquired { get; set; }
    public string? PaymentDetails { get; set; }
    public string? ORNumber { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string? Policy { get; set; }
    public string? PolicyNumber { get; set; }
    public string? PolicyID { get; set; }
    public DateTimeOffset EffectivityStart { get; set; }
    public DateTimeOffset EffectivityEnd { get; set; }
    public string? Particular { get; set; }
    public string? Building { get; set; }
    public string? Content { get; set; }
    public decimal? Premium { get; set; }
    public decimal? TotalPremium { get; set; }
    public string? Remarks { get; set; }
    public string? YearFunded { get; set; }
    public double? Allocation { get; set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public string? Uri { get; set; }
}