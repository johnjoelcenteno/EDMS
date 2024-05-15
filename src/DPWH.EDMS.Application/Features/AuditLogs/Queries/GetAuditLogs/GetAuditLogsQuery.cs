using System.Globalization;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogs;
using DPWH.EDMS.Domain.Entities;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogs;

public record GetAuditLogsQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetAuditLogsHandler : IRequestHandler<GetAuditLogsQuery, DataSourceResult>
{
    public static readonly string[] Categories = { "Inventory", "User Management", "User" };
    public const string ExpectedDateFormat = "yyyy-MM-dd";

    private readonly ILogger<GetAuditLogsHandler> _logger;
    private readonly IReadRepository _repository;
    private readonly IReadAppIdpRepository _readIdpRepository;


    public GetAuditLogsHandler(IReadRepository repository, IReadAppIdpRepository readAppIdpRepository, ILogger<GetAuditLogsHandler> logger)
    {
        _repository = repository;
        _readIdpRepository = readAppIdpRepository;
        _logger = logger;
    }

    public async Task<DataSourceResult> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var category = request.DataSourceRequest.Filter.Value.ToString();
        var (from, to) = GetDateRange((IReadOnlyList<Filter>)request.DataSourceRequest.Filter.Filters);

        if (from.Date == to.Date)
        {
            to = to.Date.AddDays(1).AddSeconds(-1);
        }

        var query = category switch
        {
            "Inventory" => _repository.ChangeLogsView
                .Include(c => c.Changes)
                .Where(c => c.ActionDate >= from && c.ActionDate <= to && c.Entity == "Asset"),

            "User Management" => _repository.ChangeLogsView
                .Include(c => c.Changes)
                .Where(c => c.ActionDate >= from && c.ActionDate <= to && c.Entity == category),

            "User" => _repository.ChangeLogsView
                .Include(c => c.Changes)
                .Where(c => c.ActionDate >= from && c.ActionDate <= to && c.Entity == "User Management" ||
                            c.ActionDate >= from && c.ActionDate <= to && c.Entity == "Asset"),

            _ => Enumerable.Empty<ChangeLog>().AsQueryable()
        };

        var entityIds = query
            .Select(c => c.EntityId)
            .ToList();

        var users = _readIdpRepository.UsersView
            .Where(x => entityIds.Contains(x.Id))
            .ToList();

        if (category == "User Management")
        {
            var result = await Task.FromResult(query
                .AsQueryable()
                .Where(c => c.Entity == "User Management" && entityIds.Contains(c.EntityId) && users.Select(u => u.Id).Contains(c.EntityId))
                .OrderByDescending(c => c.ActionDate)
                .Select(c => new GetAuditLogsQueryResult(c, users)));

            return await Task.FromResult(result.ToDataSourceResult(request.DataSourceRequest));
        }
        else if (category == "User")
        {
            var assetResults = query
                .AsQueryable()
                .Where(c => c.Entity == "User Management" && entityIds.Contains(c.EntityId) && users.Select(u => u.Id).Contains(c.EntityId) || c.Entity == "Asset")
                .OrderByDescending(c => c.ActionDate)
                .Select(c => new GetAuditLogsQueryResult(c, users));

            return await Task.FromResult(assetResults.ToDataSourceResult(request.DataSourceRequest));
        }
        else
        {
            var data = await Task.FromResult(query
            .AsSplitQuery()
            .OrderByDescending(c => c.ActionDate)
            .Select(c => new GetAuditLogsQueryResult(c, null))
            .ToDataSourceResult(request.DataSourceRequest));
            return data;
        }
    }

    private static (DateTimeOffset, DateTimeOffset) GetDateRange(IReadOnlyList<Filter> filters)
    {
        var from = filters[0].Value.ToString();
        var to = filters[1].Value.ToString();

        return (DateTimeOffset.Parse(from!, CultureInfo.InvariantCulture), DateTimeOffset.Parse(to!, CultureInfo.InvariantCulture));
    }
}