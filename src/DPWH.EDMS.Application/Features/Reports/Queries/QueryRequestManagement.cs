using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Application.Models.Reports;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Constants;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.Reports.Queries
{
    public record QueryRecordManagementRequest(DataSourceRequest Request): IRequest<DataSourceResult>;
    public class QueryRequestManagement(IReadRepository readRepository, ClaimsPrincipal claimsPrincipal) : IRequestHandler<QueryRecordManagementRequest, DataSourceResult>
    {
        private readonly IReadRepository _readRepository = readRepository;
        private readonly ClaimsPrincipal _claimsPrincipal = claimsPrincipal;

        public Task<DataSourceResult> Handle(QueryRecordManagementRequest request, CancellationToken cancellationToken)
        {
            var recordRequests = _readRepository.RecordRequestsView.Include(r => r.RequestedRecords).AsQueryable();

            var result = recordRequests
            .OrderByDescending(x => x.Created)
            .Select(s => new RecordsmanagementReportModel
            {
                Id = s.ControlNumber,
                ReleaseDate = s.DateReleased,
                Status = s.Status,
            })
            .ToDataSourceResult(request.Request.FixSerialization());

            return Task.FromResult(result);
        }
    }
}
