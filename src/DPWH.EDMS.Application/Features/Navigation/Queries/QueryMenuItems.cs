using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Navigation.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;


namespace DPWH.EDMS.Application.Features.Navigation.Queries;

public record QueryMenuItemsRequest(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

public class QueryMenuItems : IRequestHandler<QueryMenuItemsRequest, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public QueryMenuItems(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<DataSourceResult> Handle(QueryMenuItemsRequest request, CancellationToken cancellationToken)
    {
        var result = _readRepository
            .MenuItemsView
            .OrderByDescending(x => x.Created)
            .Select(x => MenuItemMappers.Map(x))
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}