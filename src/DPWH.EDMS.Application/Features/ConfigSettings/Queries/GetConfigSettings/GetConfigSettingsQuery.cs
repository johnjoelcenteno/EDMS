using DPWH.EDMS.Application.Contracts.Persistence;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Queries.GetConfigSettings;

public record GetConfigSettingsQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetConfigSettingsHandler : IRequestHandler<GetConfigSettingsQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetConfigSettingsHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetConfigSettingsQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.ConfigSettingsView
            .OrderByDescending(c => c.Created)
            .Select(c => new GetConfigSettingsQueryResult(c));

        return Task.FromResult(query.ToDataSourceResult(request.DataSourceRequest));
    }
}