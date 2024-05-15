using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Mappers;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequestById;

public record GetInspectionRequestByIdCommand(Guid Id) : IRequest<GetInspectionRequestByIdResult>;
internal class GetInspectionRequestByIdHandler : IRequestHandler<GetInspectionRequestByIdCommand, GetInspectionRequestByIdResult>
{
    public readonly IReadRepository _repository;

    public GetInspectionRequestByIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetInspectionRequestByIdResult> Handle(GetInspectionRequestByIdCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;

        var entity = await _repository.InspectionRequestsView
            .Include(x => x.Asset)
            .Include(x => x.RentalRateProperty)
            .Include(x => x.InspectionRequestBuildingComponents)
            .Include(x => x.InspectionRequestProjectMonitoring)
            .Include(x => x.InspectionRequestProjectMonitoring.InspectionRequestProjectMonitoringScopes)
            .Select(InspectionRequestMappers.MapToModelExpression())
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException("Inspection Request not found.");

        return new GetInspectionRequestByIdResult(entity);
    }
}
