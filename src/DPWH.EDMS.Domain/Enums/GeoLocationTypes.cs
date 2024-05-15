using System.ComponentModel;

namespace DPWH.EDMS.Domain.Enums;

public enum GeoLocationTypes
{
    [Description("Region Group")]
    RG,
    [Description("Region")]
    R,
    [Description("Province")]
    P,
    [Description("Congressional Disctrict Collection")]
    CC,
    [Description("Congressional District")]
    CD,
    [Description("Municipality")]
    M,
    [Description("City")]
    C,
    [Description("Barangay")]
    B
}