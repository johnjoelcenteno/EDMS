using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Mappers;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesProperty;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesPropertyById;

public record GetRentalRatesPropertyByIdQuery(Guid Id) : IRequest<RentalRatesPropertyModel>;
internal sealed class GetRentalRatesPropertyByIdHandler : IRequestHandler<GetRentalRatesPropertyByIdQuery, RentalRatesPropertyModel>
{
    private readonly IReadRepository _repository;

    public GetRentalRatesPropertyByIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<RentalRatesPropertyModel> Handle(GetRentalRatesPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.RentalRatePropertyView
                .Select(RentalRatesPropertyMappers.MapToModelExpression())
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return entity ?? throw new AppException("Rental Rate Property not found.");
    }
}
