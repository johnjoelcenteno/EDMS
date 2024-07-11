using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRecordRequestStatus;

public record UpdateRecordRequestStatus(Guid Id, string Status);