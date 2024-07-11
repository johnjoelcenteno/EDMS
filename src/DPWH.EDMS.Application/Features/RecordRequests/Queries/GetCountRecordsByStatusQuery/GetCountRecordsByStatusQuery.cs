using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Mappers;
using DPWH.EDMS.Application.Models.RecordRequests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetCountRecordsByStatusQuery;

public record GetCountRecordsByStatusQuery(string status) : IRequest<RecordRequestStatusCountModel>;

public class GetCountRecordsByStatusQueryHandler(IReadRepository readRepository) : IRequestHandler<GetCountRecordsByStatusQuery, RecordRequestStatusCountModel>
{
    private readonly IReadRepository _readRepository = readRepository;
    public async Task<RecordRequestStatusCountModel> Handle(GetCountRecordsByStatusQuery request, CancellationToken cancellationToken)
    {
        return new RecordRequestStatusCountModel
        {
            StatusName = request.status,
            Total = await _readRepository.RecordRequestsView
             .Where(rec => rec.Status == request.status).CountAsync()
        };
    }
}


