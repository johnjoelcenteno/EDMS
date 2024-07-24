using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveTransmittalReceipt;

public record class SaveTransmittalReceiptFileCommand(Guid RecordRequestId, string Filename, long FileSize, string Uri, DateTimeOffset DateReceived, DateTimeOffset TimeReceived) : IRequest<CreateResponse>;
internal class SaveTransmittalReceiptFileHandler(IWriteRepository writeRepository, IReadRepository readRepository, ClaimsPrincipal principal) : IRequestHandler<SaveTransmittalReceiptFileCommand, CreateResponse>
{
    public async Task<CreateResponse> Handle(SaveTransmittalReceiptFileCommand request, CancellationToken cancellationToken)
    {
        var model = request;

        var recordRequest = await readRepository.RecordRequestsView.FirstOrDefaultAsync(x => x.Id == model.RecordRequestId, cancellationToken);

        if (recordRequest != null)
        {
            var office = principal.GetOffice();

            var transmittalFile = RequestedRecordReceipt.Create(model.RecordRequestId, model.Filename, office, model.FileSize, model.Uri, model.DateReceived, model.TimeReceived, principal.GetUserName());

            writeRepository.RequestedRecordReceipts.Add(transmittalFile);
            await writeRepository.SaveChangesAsync(cancellationToken);

            return new CreateResponse(transmittalFile.Id);
        }

        throw new AppException("No requested record found.");
    }
}
