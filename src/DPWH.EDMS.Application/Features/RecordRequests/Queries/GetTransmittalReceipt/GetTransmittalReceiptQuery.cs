using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetTransmittalReceipt;
public record GetTransmittalReceiptQuery(Guid Id) : IRequest<IEnumerable<GetTransmittalReceiptModel>>;
internal sealed class GetTransmittalReceiptHandler(IReadRepository repository) : IRequestHandler<GetTransmittalReceiptQuery, IEnumerable<GetTransmittalReceiptModel>>
{
    public async Task<IEnumerable<GetTransmittalReceiptModel>> Handle(GetTransmittalReceiptQuery request, CancellationToken cancellationToken)
    {
        var recordReceipts = repository.RequestedRecordReceiptsView
            .Where(r => r.RecordRequestId == request.Id)
            .OrderByDescending(p => p.Created);

        var result = !recordReceipts.Any() ? Array.Empty<GetTransmittalReceiptModel>()
            : recordReceipts.Select( r => new GetTransmittalReceiptModel() { 
                Filename = r.Filename,
                Uri = r.Uri,
                Office = r.Office,
                DateReceived = r.DateReceived,
                TimeReceived = r.TimeReceived
            }).ToArray();

        return await Task.FromResult(result);
    }
}
