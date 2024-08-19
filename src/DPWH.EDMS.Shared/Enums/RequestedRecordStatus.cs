namespace DPWH.EDMS.Shared.Enums;

public enum RequestedRecordStatus
{
    Pending,
    NoRecord,
    Evaluated,
    Completed
}

public enum OfficeRequestedRecordStatus
{
    NA = 1,
    Submitted = 2,
    Reviewed = 3,
    Approved = 4,
    Released = 5,
    Claimed = 6
}