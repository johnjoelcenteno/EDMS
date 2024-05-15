using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequestsBuildingComponentsById;

public record GetInspectionRequestsBuildingComponentsById(Guid Id) : IRequest<GetInspectionRequestBuildingComponentsResult>;

internal class GetInspectionRequestsBuildingComponentsByIdHandler : IRequestHandler<GetInspectionRequestsBuildingComponentsById, GetInspectionRequestBuildingComponentsResult>
{
    private readonly IReadRepository _readRepository;

    public GetInspectionRequestsBuildingComponentsByIdHandler(IReadRepository repository)
    {
        _readRepository = repository;
    }
    public async Task<GetInspectionRequestBuildingComponentsResult> Handle(GetInspectionRequestsBuildingComponentsById request, CancellationToken cancellationToken)
    {
        var entity = await _readRepository.InspectionRequestBuildingComponentsView
               .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            throw new AppException($"Inspection request building component with Id `{request.Id}` not found");
        }

        return new GetInspectionRequestBuildingComponentsResult(entity);
    }
}
