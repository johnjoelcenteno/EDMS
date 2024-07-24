using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetTransmittalReceipt;

public record GetTransmittalReceiptQuery(Guid Id) : IRequest<GetTransmittalReceiptModel>;
internal sealed class GetTransmittalReceiptHandler(IReadRepository repository) : IRequestHandler<GetTransmittalReceiptQuery, GetTransmittalReceiptModel>
{
    public async Task<GetTransmittalReceiptModel> Handle(GetTransmittalReceiptQuery request, CancellationToken cancellationToken)
    {
        var recordRequest = await repository.RecordRequestsView
            .Include(x => x.RequestedRecordReceipt)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new AppException("No record request available.");

        return new GetTransmittalReceiptModel
        {
            Filename = recordRequest.RequestedRecordReceipt.Filename,
            Uri = recordRequest.RequestedRecordReceipt.Uri,
            DateReceived = recordRequest.RequestedRecordReceipt.DateReceived,   
            TimeReceived = recordRequest.RequestedRecordReceipt.TimeReceived
        };
    }
}
