using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesProperty;

public record GetRentalRatesPropertyQuery(DataSourceRequest Request) : IRequest<DataSourceResult>;
public class GetRentalRatesPropertyHandler : IRequestHandler<GetRentalRatesPropertyQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetRentalRatesPropertyHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetRentalRatesPropertyQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.RentalRatePropertyView
            .OrderByDescending(i => i.Created)
            .Select(RentalRatesPropertyMappers.MapToModelExpression())
            .ToDataSourceResult(request.Request.FixSerialization());

        return Task.FromResult(result);
    }
}
