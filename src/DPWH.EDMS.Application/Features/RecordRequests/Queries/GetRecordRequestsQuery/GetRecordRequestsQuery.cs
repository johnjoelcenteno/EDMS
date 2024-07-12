using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsQuery;

public record GetRecordRequestsQuery(DataSourceRequest Request) : IRequest<DataSourceResult>;
public class GetRecordRequestsQueryHandler(IReadRepository readRepository) : IRequestHandler<GetRecordRequestsQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository = readRepository;

    public async Task<DataSourceResult> Handle(GetRecordRequestsQuery request, CancellationToken cancellationToken)
    {
        var result = _readRepository.RecordRequestsView
            .Include(r => r.RequestedRecords)
            .OrderByDescending(x => x.Created)
            .Select(s => new RecordRequestSummaryModel
            {
                Id = s.Id,
                ControlNumber = s.ControlNumber,
                ClaimantType = s.ClaimantType,
                DateRequested = s.DateRequested,
                Status = s.Status,
                Purpose = s.Purpose,
                RequestedRecords = s.RequestedRecords.Select(x => new RequestedRecordSummaryModel(x.RecordTypeId, x.RecordType)),
                FullName = s.FullName,
            })
            .ToDataSourceResult(request.Request.FixSerialization());

        return await Task.FromResult(result); ;
    }
}
