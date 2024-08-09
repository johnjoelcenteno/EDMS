using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRecordRequestStatus;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRecordsRequestDocumentStatus;

public record class UpdateRecordsRequestDocumentStatusCommand(UpdateRecordsRequestDocumentStatus Model) : IRequest<UpdateResponse>;

internal sealed class UpdateRecordsRequestDocumentStatusHandler(IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal) : IRequestHandler<UpdateRecordsRequestDocumentStatusCommand, UpdateResponse>
{
    public readonly IWriteRepository _writeRepository = writeRepository;
    public readonly ClaimsPrincipal _principal = claimsPrincipal;

    public async Task<UpdateResponse> Handle(UpdateRecordsRequestDocumentStatusCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var record = _writeRepository.RequestedRecords.FirstOrDefault(x => x.Id == model.Id)
            ?? throw new AppException("No requested record found");

        var status = EnumExtensions.GetValueFromDescription<RequestedRecordStatus>(model.Status);

        record.UpdateDocumentStatus(status);

        _writeRepository.RequestedRecords.Update(record);
        await _writeRepository.SaveChangesAsync(cancellationToken);
        return new UpdateResponse(record.Id);
    }
}