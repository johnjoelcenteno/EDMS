using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRecordRequestStatus;

public record class UpdateRecordRequestStatusCommand(UpdateRecordRequestStatus Model) : IRequest<UpdateResponse>;

internal sealed class UpdateRecordRequestStatusHandler(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal) : IRequestHandler<UpdateRecordRequestStatusCommand, UpdateResponse>
{
    public readonly IWriteRepository _writeRepository = writeRepository;
    public readonly ClaimsPrincipal _principal = claimsPrincipal;

    public async Task<UpdateResponse> Handle(UpdateRecordRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var record = _writeRepository.RecordRequests.FirstOrDefault(x => x.Id == model.Id) 
            ?? throw new AppException("No record request found");

        var status = EnumExtensions.GetValueFromDescription<RecordRequestStates>(model.Status);

        record.UpdateStatus(status, _principal.GetUserName());

        _writeRepository.RecordRequests.Update(record);
        await _writeRepository.SaveChangesAsync(cancellationToken);
        return new UpdateResponse(record.Id);
    }
}
