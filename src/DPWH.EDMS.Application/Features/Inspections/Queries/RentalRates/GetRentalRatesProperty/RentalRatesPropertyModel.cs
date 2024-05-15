using DPWH.EDMS.Application.Features.Assets.Queries;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesProperty;

public class RentalRatesPropertyModel
{
    public Guid Id { get; set; }
    public string RentalRateNumber { get; set; }
    public string PropertyName { get; set; }
    public string? TypeOfStructure { get; set; }
    public string? ImplementingOffice { get; set; }
    public string? Group { get; set; }
    public string? Agency { get; set; }
    public string? AttachedAgency { get; set; }
    public string? Region { get; set; }
    public string? RegionId { get; set; }
    public string? Province { get; set; }
    public string? ProvinceId { get; set; }
    public string? CityOrMunicipality { get; set; }
    public string? CityOrMunicipalityId { get; set; }
    public string? Barangay { get; set; }
    public string? BarangayId { get; set; }
    public string? StreetAddress { get; set; }
    public LongLatFormat? Longitude { get; set; }
    public LongLatFormat? Latitude { get; set; }
}