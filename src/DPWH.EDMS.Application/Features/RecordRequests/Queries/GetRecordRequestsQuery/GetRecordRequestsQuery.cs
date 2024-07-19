using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsQuery;

public record GetRecordRequestsQuery(DataSourceRequest Request) : IRequest<DataSourceResult>;
public class GetRecordRequestsQueryHandler(IReadRepository readRepository, ClaimsPrincipal claimsPrincipal) : IRequestHandler<GetRecordRequestsQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository = readRepository;
    private readonly ClaimsPrincipal _claimsPrincipal = claimsPrincipal;

    public async Task<DataSourceResult> Handle(GetRecordRequestsQuery request, CancellationToken cancellationToken)
    {
        var recordRequests = _readRepository.RecordRequestsView.Include(r => r.RequestedRecords).AsQueryable();

        if(_claimsPrincipal.IsInRole(ApplicationRoles.Staff) || _claimsPrincipal.IsInRole(ApplicationRoles.Manager))
        {
            recordRequests = recordRequests.Where(r => r.RequestedRecords.Any(rr => rr.Office == _claimsPrincipal.GetOffice()));
        }

        var result = recordRequests
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

        return await Task.FromResult(result);
    }
}
