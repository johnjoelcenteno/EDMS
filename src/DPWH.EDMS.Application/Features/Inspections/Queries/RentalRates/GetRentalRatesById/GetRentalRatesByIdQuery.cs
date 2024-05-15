using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Mappers;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRates;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesById;

public record GetRentalRatesByIdQuery(Guid Id) : IRequest<RentalRatesModel>;

internal sealed class GetRentalRatesByIdHandler : IRequestHandler<GetRentalRatesByIdQuery, RentalRatesModel>
{
    private readonly IReadRepository _repository;

    public GetRentalRatesByIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<RentalRatesModel> Handle(GetRentalRatesByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.RentalRatesView
                        .Include(x => x.Images)
                        .Include(x => x.Files)
                        .Select(RentalRatesMappers.MapToModelExpression())
                        .FirstOrDefaultAsync(x => x.RentalRatesPropertyId == request.Id, cancellationToken);

        return entity ?? throw new AppException("Rental Rate not found.");
    }
}
