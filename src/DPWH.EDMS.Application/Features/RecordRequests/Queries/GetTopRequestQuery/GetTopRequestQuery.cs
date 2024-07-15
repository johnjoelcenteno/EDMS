using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetMonthlyRequests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetTopRequestQuery;
public record GetTopRequestQuery() : IRequest<IEnumerable<GetTopRequestQueryModel>>;
internal sealed class GetTopRequestQueryHandler(IReadRepository repository) : IRequestHandler<GetTopRequestQuery, IEnumerable<GetTopRequestQueryModel>>
{
    private readonly IReadRepository _readRepository = repository;


    public async Task<IEnumerable<GetTopRequestQueryModel>> Handle(GetTopRequestQuery request, CancellationToken cancellationToken)
    {
        var topRequest = await _readRepository.RecordRequestsView
            .SelectMany(x => x.RequestedRecords)
            .GroupBy(f => f.RecordType)
            .OrderByDescending(g => g.Count())
            .Take(10).Select(t => new GetTopRequestQueryModel
            {
                RecordName = t.Key,
                Total = t.Count()
            }).ToListAsync();

        return topRequest;
    }
}