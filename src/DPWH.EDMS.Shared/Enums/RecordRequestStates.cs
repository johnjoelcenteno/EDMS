using System.ComponentModel;

namespace DPWH.EDMS.Shared.Enums;

public enum RecordRequestStates
{
    Submitted,
    Reviewed,
    Approved,
    [Description("For Release")]
    ForRelease,
    Claimed
}