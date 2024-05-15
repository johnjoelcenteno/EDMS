using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class RentalRateProperty : EntityBase
{
    private RentalRateProperty()
    {

    }

    private RentalRateProperty(string rentalRateNumber, string propertyName, string? typeStructure, string? implementingOffice, string? group, string? agency, string? attachedAgency, string? region, string? regionId, string? province, string? provinceId, string? city, string? cityId, string? streetAddress, double? longDegrees, double? longMinutes, double? longSeconds, string? longDirection, double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection)
    {
        RentalRateNumber = rentalRateNumber;
        PropertyName = propertyName;
        TypeOfStructure = typeStructure;
        ImplementingOffice = implementingOffice;
        Group = group;
        Agency = agency;
        AttachedAgency = attachedAgency;
        Region = region;
        RegionId = regionId;
        Province = province;
        ProvinceId = provinceId;
        CityOrMunicipality = city;
        CityOrMunicipalityId = cityId;
        //Barangay = barangay;
        //BarangayId = barangayId;
        StreetAddress = streetAddress;
        LongDegrees = longDegrees;
        LongMinutes = longMinutes;
        LongSeconds = longSeconds;
        LongDirection = longDirection;
        LatDegrees = latDegrees;
        LatMinutes = latMinutes;
        LatSeconds = latSeconds;
        LatDirection = latDirection;
    }

    public static RentalRateProperty Create(string rentalRateNumber, string propertyName, string? typeStructure, string? implementingOffice, string? group, string? agency, string? attachedAgency, string? region, string? regionId, string? province, string? provinceId, string? city, string? cityId, string? streetAddress, double? longDegrees, double? longMinutes, double? longSeconds, string? longDirection, double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection, string createdBy)
    {
        var entity = new RentalRateProperty(rentalRateNumber, propertyName, typeStructure, implementingOffice, group, agency, attachedAgency, region, regionId, province, provinceId, city, cityId, streetAddress, longDegrees, longMinutes, longSeconds, longDirection, latDegrees, latMinutes, latSeconds, latDirection);

        entity.SetCreated(createdBy);
        return entity;
    }

    public void Update(string propertyName, string? typeStructure, string? implementingOffice, string? group, string? agency, string? attachedAgency, string? region, string? regionId, string? province, string? provinceId, string? city, string? cityId, string? streetAddress, double? longDegrees, double? longMinutes, double? longSeconds, string? longDirection, double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection, string modifiedBy)
    {
        PropertyName = propertyName;
        TypeOfStructure = typeStructure;
        ImplementingOffice = implementingOffice;
        Group = group;
        Agency = agency;
        AttachedAgency = attachedAgency;
        Region = region;
        RegionId = regionId;
        Province = province;
        ProvinceId = provinceId;
        CityOrMunicipality = city;
        CityOrMunicipalityId = cityId;
        //Barangay = barangay;
        //BarangayId = barangayId;
        StreetAddress = streetAddress;
        LongDegrees = longDegrees;
        LongMinutes = longMinutes;
        LongSeconds = longSeconds;
        LongDirection = longDirection;
        LatDegrees = latDegrees;
        LatMinutes = latMinutes;
        LatSeconds = latSeconds;
        LatDirection = latDirection;

        SetModified(modifiedBy);
    }

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
    public double? LongDegrees { get; set; }
    public double? LongMinutes { get; set; }
    public double? LongSeconds { get; set; }
    public string? LongDirection { get; set; }
    public double? LatDegrees { get; set; }
    public double? LatMinutes { get; set; }
    public double? LatSeconds { get; set; }
    public string? LatDirection { get; set; }
}
