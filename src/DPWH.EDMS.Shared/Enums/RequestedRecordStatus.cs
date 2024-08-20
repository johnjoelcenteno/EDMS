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
    NA,
    Submitted = 1,
    Reviewed = 2,
    Approved = 3,
    Released = 4,
    Claimed = 5
}