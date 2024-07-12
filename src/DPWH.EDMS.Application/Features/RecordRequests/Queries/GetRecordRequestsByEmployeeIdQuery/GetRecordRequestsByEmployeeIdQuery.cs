using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsByEmployeeIdQuery;

public record GetRecordRequestsByEmployeeIdQuery(DataSourceRequest Request, string EmployeeId) : IRequest<DataSourceResult>;
public class GetRecordRequestsByEmployeeIdQueryHandler(IReadRepository readRepository) : IRequestHandler<GetRecordRequestsByEmployeeIdQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository = readRepository;

    public async Task<DataSourceResult> Handle(GetRecordRequestsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var result = _readRepository.RecordRequestsView
            .Include(r => r.RequestedRecords)
            .Where(p => p.EmployeeNumber == request.EmployeeId)
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
                RequestedRecords = s.RequestedRecords.Select(x => new RequestedRecordSummaryModel(x.RecordTypeId, x.RecordType))
            })
            .ToDataSourceResult(request.Request.FixSerialization());

        return await Task.FromResult(result); ;
    }
}
