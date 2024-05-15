namespace DPWH.EDMS.Application.Features.ArcGis.Commands.BatchCreateArcgis;

public static class DmsToLongLat
{
    public static double? Convert(DmsPoint dmsPoint)
    {
        // Convert minutes and seconds to decimal
        var decimalMinutes = dmsPoint.Minutes / 60.0;
        var decimalSeconds = dmsPoint.Seconds / 3600.0;

        // Combine degrees, minutes, and seconds to get the final decimal degrees
        var decimalDegrees = dmsPoint.Degrees + decimalMinutes + decimalSeconds;

        // Determine the sign based on the direction (North or South)
        if (dmsPoint.Direction != null && Enum.TryParse(dmsPoint.Direction, out Direction direction))
        {
            // Use 'direction' within this block
            decimalDegrees = direction == Direction.S ? -decimalDegrees : decimalDegrees;
        }

        return decimalDegrees;
    }
}

public class DmsPoint
{
    public double? Degrees { get; set; }
    public double? Minutes { get; set; }
    public double? Seconds { get; set; }
    public PointType Type { get; set; }
    public string? Direction { get; set; }

}

public enum PointType
{
    Lat,
    Lon
}
public enum Direction
{
    N,
    S,
    W,
    E
}