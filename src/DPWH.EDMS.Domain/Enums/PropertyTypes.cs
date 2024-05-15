using System.ComponentModel;

namespace DPWH.EDMS.Domain.Enums;

public enum PropertyStatus
{
    Good,
    [Description("Minor Repair")]
    MinorRepair,
    [Description("Major Repair")]
    MajorRepair,
    Rehabilitation,
    [Description("Total Rehabilitation")]
    TotalRehabilitation,
    Reconstruction,
    [Description("For Demolition")]
    ForDemolition,
    Abandoned,
    Unserviceable
}

