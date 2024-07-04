using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsByStatusQuery;

public record GetRecordRequestsByStatusQuery(DataSourceRequest Request, string Status) : IRequest<DataSourceResult>;
internal class GetRecordRequestsByStatusQueryHandler(IReadRepository readRepository) : IRequestHandler<GetRecordRequestsByStatusQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository = readRepository;

    public async Task<DataSourceResult> Handle(GetRecordRequestsByStatusQuery request, CancellationToken cancellationToken)
    {
        var result = _readRepository.RecordRequestsView
                    .Include(x => x.RequestedRecords)
                    .Where(x => x.Status == request.Status)
                    .OrderByDescending(x => x.Created)
                    .Select(s => new RecordRequestSummaryModel
                    {
                        Id = s.Id,
                        ControlNumber = s.ControlNumber,
                        ClaimantType = s.ClaimantType,
                        DateRequested = s.DateRequested,
                        Status = s.Status,
                        Purpose = s.Purpose,
                        FullName = s.FullName,
                        RequestedRecords = s.RequestedRecords.Select(x => new RequestedRecordModel(x.RecordTypeId, x.RecordType))
                    }).ToDataSourceResult(request.Request.FixSerialization());

        return await Task.FromResult(result);
    }
}