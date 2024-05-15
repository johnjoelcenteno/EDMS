namespace DPWH.EDMS.Application.Features.RequestingOffices.Commands.BatchCreateRequestingOffice;

public record RegionalOffice(string NumberCode, string Name)
{
    public static IReadOnlyCollection<RegionalOffice> List => new[]
    {
        new RegionalOffice("R01", "Region I"),
        new RegionalOffice("R02", "Region II"),
        new RegionalOffice("R03", "Region III"),
        new RegionalOffice("R4A", "Region IV-A"),
        new RegionalOffice("R4B", "Region IV-B"),
        new RegionalOffice("R05", "Region V"),
        new RegionalOffice("R06", "Region VI"),
        new RegionalOffice("R07", "Region VII"),
        new RegionalOffice("R08", "Region VIII"),
        new RegionalOffice("R09", "Region IX"),
        new RegionalOffice("R10", "Region X"),
        new RegionalOffice("R11", "Region XI"),
        new RegionalOffice("R12", "Region XII"),
        new RegionalOffice("R13", "Region XIII"),
        new RegionalOffice("CAR", "Cordillera Administrative Region"),
        new RegionalOffice("NCR", "National Capital Region")
    };
}