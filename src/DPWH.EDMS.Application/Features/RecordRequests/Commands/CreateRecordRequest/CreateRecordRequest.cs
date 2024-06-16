﻿namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;
public record CreateRecordRequest(string EmployeeNumber, string ControlNumber, bool IsActiveEmployee,
    string Claimant, DateTimeOffset DateRequested, string? AuthorizedRepresentative, Guid? ValidId, Guid? SupportingDocument,
    Guid[] RequestedRecords, string Purpose);

