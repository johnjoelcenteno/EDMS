using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class Asset : EntityBase
{
    public Asset() { }

    public static Asset Create(string? propertyId, string? name, string propertyStatus, string? regionId, string? region, string? provinceId, string? province,
        string? cityOrMunicipalityId, string? cityOrMunicipality, string? barangayId, string? barangay, string? zipCode, string? streetAddress, double? longDegrees, double? longMinutes, double? longSeconds, string? longDirection, double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection, decimal? lotArea,
        decimal? floorArea, int floors, decimal zonalValue, string createdBy, string? buildingId, string? attachedAgency, string? agency, string? group, string? constructionType, string? lotStatus, string? buildingStatus, decimal bookValue, decimal appraisedValue, int yearConstruction, int? monthConstruction, int? dayConstruction, string? oldId, string? remarks, string? implementingOffice,
        string requestingOffice)
    {
        var asset = new Asset
        {
            PropertyId = propertyId,
            BuildingId = buildingId,
            Name = name,
            Agency = agency,
            ImplementingOffice = implementingOffice,
            AttachedAgency = attachedAgency,
            Group = group,
            Status = AssetStatus.Draft.ToString(),
            PropertyStatus = propertyStatus.ToString(),
            Region = region,
            RegionId = regionId,
            Province = province,
            ProvinceId = provinceId,
            CityOrMunicipality = cityOrMunicipality,
            CityOrMunicipalityId = cityOrMunicipalityId,
            Barangay = barangay,
            BarangayId = barangayId,
            ZipCode = zipCode,
            StreetAddress = streetAddress,
            LongDegrees = longDegrees,
            LongMinutes = longMinutes,
            LongSeconds = longSeconds,
            LongDirection = longDirection,
            LatDegrees = latDegrees,
            LatMinutes = latMinutes,
            LatSeconds = latSeconds,
            LatDirection = latDirection,
            LotArea = lotArea,
            FloorArea = floorArea,
            Floors = floors,
            YearConstruction = yearConstruction,
            MonthConstruction = monthConstruction,
            DayConstruction = dayConstruction,
            ConstructionType = constructionType,
            LotStatus = lotStatus,
            BuildingStatus = buildingStatus,
            ZonalValue = zonalValue,
            BookValue = bookValue,
            AppraisedValue = appraisedValue,
            OldId = oldId,
            Remarks = remarks,
            RequestingOffice = requestingOffice
        };

        asset.SetCreated(createdBy);
        return asset;
    }

    public void UpdateDetails(string? propertyId, string name, string propertyStatus, string region, string regionId, string province, string provinceId,
        string cityOrMunicipality, string cityOrMunicipalityId, string barangay, string? barangayId, string zipCode, string streetAddress, double? longDegrees, double? longMinutes, double? longSeconds, string? longDirection, double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection, decimal? lotArea, decimal? floorArea, int floors,
        decimal zonalValue, string modifiedBy, string? attachedAgency, string? agency, string? group, string? constructionType, string? lotStatus, string? buildingStatus, decimal bookValue, decimal appraisedValue, int yearConstruction, int? monthConstruction, int? dayConstruction, string? implementingOffice, string? requestingOffice)
    {
        PropertyId = propertyId;
        Name = name;
        PropertyStatus = propertyStatus.ToString();
        AttachedAgency = attachedAgency;
        Agency = agency;
        Group = group;
        Region = region;
        RegionId = regionId;
        Province = province;
        ProvinceId = provinceId;
        CityOrMunicipality = cityOrMunicipality;
        CityOrMunicipalityId = cityOrMunicipalityId;
        Barangay = barangay;
        BarangayId = barangayId;
        ZipCode = zipCode;
        StreetAddress = streetAddress;
        LongDegrees = longDegrees;
        LongMinutes = longMinutes;
        LongSeconds = longSeconds;
        LongDirection = longDirection;
        LatDegrees = latDegrees;
        LatMinutes = latMinutes;
        LatSeconds = latSeconds;
        LatDirection = latDirection;
        LotArea = lotArea;
        FloorArea = floorArea;
        Floors = floors;
        YearConstruction = yearConstruction;
        MonthConstruction = monthConstruction;
        DayConstruction = dayConstruction;
        ConstructionType = constructionType;
        LotStatus = lotStatus;
        BuildingStatus = buildingStatus;
        ZonalValue = zonalValue;
        BookValue = bookValue;
        AppraisedValue = appraisedValue;
        ImplementingOffice = implementingOffice;
        RequestingOffice = requestingOffice;

        SetModified(modifiedBy);
    }

    public void Update(string? propertyId, string name, string propertyStatus, string region, string regionId, string province, string provinceId,
        string cityOrMunicipality, string cityOrMunicipalityId, string barangay, string? barangayId, string zipCode, string streetAddress, decimal? lotArea, decimal? floorArea, int floors,
        decimal zonalValue, string modifiedBy, string? attachedAgency, string? agency, string? group, string? constructionType, string? lotStatus, string? buildingStatus, decimal bookValue, decimal appraisedValue, int yearConstruction, int? monthConstruction, int? dayConstruction, string? implementingOffice, string? requestingOffice)
    {
        PropertyId = propertyId;
        Name = name;
        PropertyStatus = propertyStatus.ToString();
        AttachedAgency = attachedAgency;
        Agency = agency;
        Group = group;
        Region = region;
        RegionId = regionId;
        Province = province;
        ProvinceId = provinceId;
        CityOrMunicipality = cityOrMunicipality;
        CityOrMunicipalityId = cityOrMunicipalityId;
        Barangay = barangay;
        BarangayId = barangayId;
        ZipCode = zipCode;
        StreetAddress = streetAddress;
        LotArea = lotArea;
        FloorArea = floorArea;
        Floors = floors;
        YearConstruction = yearConstruction;
        MonthConstruction = monthConstruction;
        DayConstruction = dayConstruction;
        ConstructionType = constructionType;
        LotStatus = lotStatus;
        BuildingStatus = buildingStatus;
        ZonalValue = zonalValue;
        BookValue = bookValue;
        AppraisedValue = appraisedValue;
        ImplementingOffice = implementingOffice;
        RequestingOffice = requestingOffice;

        SetModified(modifiedBy);
    }

    public void Update(AssetStatus status, string updatedBy)
    {
        Status = status.ToString();
        SetModified(updatedBy);
    }

    public Asset Clone()
    {
        Asset clonedAsset = new Asset
        {
            PropertyId = PropertyId,
            BuildingId = BuildingId,
            Name = Name,
            RequestingOffice = RequestingOffice,
            ImplementingOffice = ImplementingOffice,
            Agency = Agency,
            Group = Group,
            Status = Status,
            PropertyStatus = PropertyStatus,
            AttachedAgency = AttachedAgency,
            Region = Region,
            RegionId = RegionId,
            Province = Province,
            ProvinceId = ProvinceId,
            CityOrMunicipality = CityOrMunicipality,
            CityOrMunicipalityId = CityOrMunicipalityId,
            Barangay = Barangay,
            BarangayId = BarangayId,
            ZipCode = ZipCode,
            StreetAddress = StreetAddress,
            LongDegrees = LongDegrees,
            LongMinutes = LongMinutes,
            LongSeconds = LongSeconds,
            LongDirection = LongDirection,
            LatDegrees = LatDegrees,
            LatMinutes = LatMinutes,
            LatSeconds = LatSeconds,
            LatDirection = LatDirection,
            LotArea = LotArea,
            FloorArea = FloorArea,
            Floors = Floors,
            YearConstruction = YearConstruction,
            MonthConstruction = MonthConstruction,
            DayConstruction = DayConstruction,
            ConstructionType = ConstructionType,
            LotStatus = LotStatus,
            BuildingStatus = BuildingStatus,
            ZonalValue = ZonalValue,
            BookValue = BookValue,
            AppraisedValue = AppraisedValue,
            OldId = OldId,
            Remarks = Remarks
        };

        if (FinancialDetails != null)
        {
            clonedAsset.FinancialDetails = new FinancialDetails
            {
                PaymentDetails = FinancialDetails.PaymentDetails,
                ORNumber = FinancialDetails.ORNumber,
                PaymentDate = FinancialDetails.PaymentDate,
                AmountPaid = FinancialDetails.AmountPaid,
                Policy = FinancialDetails.Policy,
                PolicyNumber = FinancialDetails.PolicyNumber,
                PolicyID = FinancialDetails.PolicyID,
                EffectivityStart = FinancialDetails.EffectivityStart,
                Particular = FinancialDetails.Particular,
                Building = FinancialDetails.Building,
                Content = FinancialDetails.Content,
                Premium = FinancialDetails.Premium,
                TotalPremium = FinancialDetails.TotalPremium,
                Remarks = FinancialDetails.Remarks,
                EffectivityEnd = FinancialDetails.EffectivityEnd
            };
        }
        return clonedAsset;
    }

    public override bool Equals(object? asset)
    {
        if (asset == null || GetType() != asset.GetType())
        {
            return false;
        }

        Asset other = (Asset)asset;

        return PropertyId == other.PropertyId &&
               BuildingId == other.BuildingId &&
               Name == other.Name &&
               RequestingOffice == other.RequestingOffice &&
               ImplementingOffice == other.ImplementingOffice &&
               Agency == other.Agency &&
               Group == other.Group &&
               Status == other.Status &&
               PropertyStatus == other.PropertyStatus &&
               AttachedAgency == other.AttachedAgency &&
               Region == other.Region &&
               RegionId == other.RegionId &&
               Province == other.Province &&
               ProvinceId == other.ProvinceId &&
               CityOrMunicipality == other.CityOrMunicipality &&
               CityOrMunicipalityId == other.CityOrMunicipalityId &&
               Barangay == other.Barangay &&
               BarangayId == other.BarangayId &&
               ZipCode == other.ZipCode &&
               StreetAddress == other.StreetAddress &&
               LongDegrees == other.LongDegrees &&
               LongMinutes == other.LongMinutes &&
               LongSeconds == other.LongSeconds &&
               LongDirection == other.LongDirection &&
               LatDegrees == other.LatDegrees &&
               LatMinutes == other.LatMinutes &&
               LatSeconds == other.LatSeconds &&
               LatDirection == other.LatDirection &&
               LotArea == other.LotArea &&
               FloorArea == other.FloorArea &&
               Floors == other.Floors &&
               YearConstruction == other.YearConstruction &&
               MonthConstruction == other.MonthConstruction &&
               DayConstruction == other.DayConstruction &&
               ConstructionType == other.ConstructionType &&
               LotStatus == other.LotStatus &&
               BuildingStatus == other.BuildingStatus &&
               ZonalValue == other.ZonalValue &&
               BookValue == other.BookValue &&
               AppraisedValue == other.AppraisedValue &&
               OldId == other.OldId &&
               Remarks == other.Remarks &&
               FinancialDetailsEquals(other.FinancialDetails);
    }
    private bool FinancialDetailsEquals(FinancialDetails? other)
    {
        if (FinancialDetails == null && other == null)
        {
            return true;
        }
        else if (FinancialDetails == null || other == null)
        {
            return false;
        }

        return FinancialDetails.PaymentDetails == other.PaymentDetails &&
               FinancialDetails.ORNumber == other.ORNumber &&
               FinancialDetails.PaymentDate == other.PaymentDate &&
               FinancialDetails.AmountPaid == other.AmountPaid &&
               FinancialDetails.Policy == other.Policy &&
               FinancialDetails.PolicyNumber == other.PolicyNumber &&
               FinancialDetails.PolicyID == other.PolicyID &&
               FinancialDetails.EffectivityStart == other.EffectivityStart &&
               FinancialDetails.EffectivityEnd == other.EffectivityEnd &&
               FinancialDetails.Particular == other.Particular &&
               FinancialDetails.Building == other.Building &&
               FinancialDetails.Content == other.Content &&
               FinancialDetails.Premium == other.Premium &&
               FinancialDetails.TotalPremium == other.TotalPremium &&
               FinancialDetails.Remarks == other.Remarks;
    }

    public string? PropertyId { get; set; }
    public string? BuildingId { get; set; }
    public string? Name { get; set; }
    public string? RequestingOffice { get; set; }
    public string? ImplementingOffice { get; set; }
    public string? Agency { get; set; }
    public string? Group { get; set; }
    public string Status { get; set; }
    public string PropertyStatus { get; set; }
    public string? AttachedAgency { get; set; }
    public string? Region { get; set; }
    public string? RegionId { get; set; }
    public string? Province { get; set; }
    public string? ProvinceId { get; set; }
    public string? CityOrMunicipality { get; set; }
    public string? CityOrMunicipalityId { get; set; }
    public string? Barangay { get; set; }
    public string? BarangayId { get; set; }
    public string? ZipCode { get; set; }
    public string? StreetAddress { get; set; }
    public double? LongDegrees { get; set; }
    public double? LongMinutes { get; set; }
    public double? LongSeconds { get; set; }
    public string? LongDirection { get; set; }
    public double? LatDegrees { get; set; }
    public double? LatMinutes { get; set; }
    public double? LatSeconds { get; set; }
    public string? LatDirection { get; set; }
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
    public decimal AppraisedValue { get; set; }
    public decimal BookValue { get; set; }
    public string? OldId { get; set; }
    public string? Remarks { get; set; }
    public virtual IList<AssetImageDocument> Images { get; set; }
    public virtual IList<AssetFileDocument>? Files { get; set; }
    public virtual FinancialDetails? FinancialDetails { get; set; }
    public virtual IList<FinancialDetailsDocuments> FinancialDetailsDocuments { get; set; }
    public virtual IList<InspectionRequest> InspectionRequests { get; set; }
}
