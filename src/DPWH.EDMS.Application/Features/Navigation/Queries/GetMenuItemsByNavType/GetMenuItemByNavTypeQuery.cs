using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Navigation.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Navigation.Queries.GetMenuItemsByNavType;

public record QueryMenuItemsByNavTypeRequest(string navType, DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

public class QueryMenuItemsByNavType : IRequestHandler<QueryMenuItemsByNavTypeRequest, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public QueryMenuItemsByNavType(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<DataSourceResult> Handle(QueryMenuItemsByNavTypeRequest request, CancellationToken cancellationToken)
    {
        var result = _readRepository
            .MenuItemsView
            .Where(x => x.NavType == request.navType)
            .OrderByDescending(x => x.Created)
            .Select(x => MenuItemMappers.Map(x))
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}