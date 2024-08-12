using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRecordRequestStatus;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateOfficeStatus;

public record class UpdateOfficeStatusCommand(UpdateOfficeStatus Model) : IRequest<UpdateResponse>;

internal sealed class UpdateOfficeStatusHandler(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal) : IRequestHandler<UpdateOfficeStatusCommand, UpdateResponse>
{
    public readonly IWriteRepository _writeRepository = writeRepository;
    public readonly ClaimsPrincipal _principal = claimsPrincipal;

    public async Task<UpdateResponse> Handle(UpdateOfficeStatusCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var record = _writeRepository.RecordRequests.FirstOrDefault(x => x.Id == model.Id)
            ?? throw new AppException("No record request found");
         
        var status = EnumExtensions.GetValueFromDescription<OfficeRequestedRecordStatus>(model.Status);
        var office = EnumExtensions.GetValueFromName<Offices>(_principal.GetOffice()!);

        record.UpdateOfficeStatus(status, _principal.GetUserName(), office);

        await _writeRepository.SaveChangesAsync(cancellationToken);
        return new UpdateResponse(record.Id);
    }
}

