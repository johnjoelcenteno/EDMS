using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetCountRecordsByStatusQuery;

public record GetCountRecordsByStatusQuery(string status) : IRequest<int>;

public class GetCountRecordsByStatusQueryHandler(IReadRepository readRepository) : IRequestHandler<GetCountRecordsByStatusQuery, int>
{
    private readonly IReadRepository _readRepository = readRepository;
    public async Task<int> Handle(GetCountRecordsByStatusQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.RecordRequestsView.Where(rec => rec.Status == request.status).CountAsync();
    }
}


