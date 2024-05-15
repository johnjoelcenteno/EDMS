using System.ComponentModel;

namespace DPWH.EDMS.Domain.Enums;

public enum RentalRateType
{
    [Description("Location and Site Conditions")]
    LocationAndSiteConditions,
    [Description("Neighborhood Data")]
    NeighborhoodData,
    [Description("Building")]
    Building,
    [Description("Free Services and Facilities")]
    FreeServicesAndFacilities,
}
