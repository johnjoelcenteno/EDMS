﻿namespace DPWH.EDMS.Application.Features.RecordRequests.Queries;
public record RecordRequestSummaryModel
{
    public Guid Id { get; set; }
    public int ControlNumber { get; set; }
    public string ClaimantType { get; set; }
    public DateTimeOffset? DateRequested { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
    public string? HRMDRequestStatus { get; set; }
    public string? RMDRequestStatus { get; set; }
    public IEnumerable<RequestedRecordSummaryModel> RequestedRecords { get; set; } = [];
    public string? FullName { get; set; }
    public DateTimeOffset? DateReleased { get; set; }
}
