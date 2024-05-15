using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application.Features.Reports.Queries;

public class ReportModel
{
    public string? NameOfItem { get; set; }
    public int NumberOfItems { get; set; }
}

public class AssestPerConditionModel : ReportModel
{

}

public class AssetsPerBuildingStatus : ReportModel
{

}

public class InventoryBuildingDetails : AuditableModel
{
    public string? BuildingId { get; set; }
    public string? BuildingName { get; set; }
    public string? PropertyId { get; set; }
}

public class InventoryReport : AuditableModel
{
    public string? PropertyId { get; set; }
    public string? PropertyName { get; set; }
}

public class InventoryReportByFinancialDetails : InventoryReport
{
    public decimal ZonalValue { get; set; }
}

public class InventoryReportByArea : InventoryBuildingDetails
{
    public decimal? LotArea { get; set; }
    public decimal? FloorArea { get; set; }
}
public class InventoryReportByLocation : InventoryBuildingDetails
{
    public string? Region { get; set; }
    public string? RegionId { get; set; }
    public string? Province { get; set; }
    public string? ProvinceId { get; set; }
    public string? CityOrMunicipality { get; set; }
    public string? CityOrMunicipalityId { get; set; }
    public string? Barangay { get; set; }
    public string? BarangayId { get; set; }
    public string? ZipCode { get; set; }
}
public class InventoryReportByFundingHistory
{
    public string? YearFunded { get; set; }
    public double? Allocation { get; set; }
    public string? Uri { get; set; }
}
public class InventoryReportByFundingHistoryResult : InventoryBuildingDetails
{
    public string? YearFunded { get; set; }
    public double? Allocation { get; set; }
    public string? Uri { get; set; }
}

public class InventoryReportByPropertyDetails : AuditableModel
{
    public string? BuildingId { get; set; }
    public string? BuildingName { get; set; }
    public string? PropertyId { get; set; }
    public string? Region { get; set; }
    public string? ImplementingOffice { get; set; }
    public string? RequestingOffice { get; set; }
    public string? Agency { get; set; }
    public string? Group { get; set; }
    public string? Province { get; set; }
    public string? CityOrMunicipality { get; set; }
    public string? ZipCode { get; set; }
    public string? StreetAddress { get; set; }
    public string? PropertyCondition { get; set; }
    public string? BuildingStatus { get; set; }
    public decimal? LotArea { get; set; }
    public decimal? FloorArea { get; set; }
    public int Floors { get; set; }
    public string? ConstructionType { get; set; }
    public int YearConstruction { get; set; }
    public decimal BookValue { get; set; }
    public decimal AppraisedValue { get; set; }
    public string? LotStatus { get; set; }
    public decimal ZonalValue { get; set; }
}

public class PriReport : AuditableModel
{
    public string Region { get; set; }
    public string ImplementingOffice { get; set; }
    public string ProjectName { get; set; }
    public string Location { get; set; }
}

public static class ModelHelper
{
    public static T CalculateTotal<T>(List<T> values)
    {
        dynamic sum = 0;
        foreach (var value in values)
        {
            sum += value;
        }
        return sum;
    }
}