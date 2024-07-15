using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveRequestedRecordFile;

public record class SaveRequestedRecordFileCommand(Guid Id, string Uri) : IRequest<CreateResponse>;
internal sealed class SaveRequestedRecordFileHandler(IWriteRepository writeRepository, ClaimsPrincipal principal) : IRequestHandler<SaveRequestedRecordFileCommand, CreateResponse>
{
    public async Task<CreateResponse> Handle(SaveRequestedRecordFileCommand request, CancellationToken cancellationToken)
    {
        var model = request;

        var requestedRecord = await writeRepository.RequestedRecords.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken) 
            ?? throw new AppException("No requested record available");

        requestedRecord.Update(model.Uri);

        await writeRepository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(requestedRecord.Id);
    }
}
