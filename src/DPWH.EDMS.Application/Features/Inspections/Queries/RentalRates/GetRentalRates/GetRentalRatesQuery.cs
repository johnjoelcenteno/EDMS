using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRates;

public record GetRentalRatesQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetRentalRatesHandler : IRequestHandler<GetRentalRatesQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetRentalRatesHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetRentalRatesQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.RentalRatesView
            .Include(x => x.Images)
            .Include(x => x.Files)
            .OrderByDescending(i => i.Created)
            .Select(RentalRatesMappers.MapToModelExpression())
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}
