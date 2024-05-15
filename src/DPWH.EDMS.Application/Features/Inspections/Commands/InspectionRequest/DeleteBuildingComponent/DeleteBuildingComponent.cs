using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.DeleteBuildingComponent;

public record DeleteBuildingComponentCommand : IRequest<DeleteBuildingComponentResult>
{
    public Guid InspectionRequestId { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
}
internal sealed class DeleteBuildingComponent : IRequestHandler<DeleteBuildingComponentCommand, DeleteBuildingComponentResult>
{
    private readonly IWriteRepository _repository;

    public DeleteBuildingComponent(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteBuildingComponentResult> Handle(DeleteBuildingComponentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.InspectionRequests
        .Include(i => i.InspectionRequestBuildingComponents)
            .FirstOrDefaultAsync(x => x.Id == request.InspectionRequestId, cancellationToken)
            ?? throw new AppException($"Inspection request `{request.InspectionRequestId}` not found");

        var inspectionRequestBuildingComponents = await _repository.InspectionRequestBuildingComponents
             .Where(x => x.InspectionRequestId == entity.Id)
             .ToListAsync(cancellationToken);

        var buildingComponent = inspectionRequestBuildingComponents.FirstOrDefault(x => x.Category == request.Category && x.SubCategory == request.SubCategory)
            ?? throw new AppException($"SubCategory `{request.SubCategory}` not found.");

        _repository.InspectionRequestBuildingComponents.Remove(buildingComponent);
        await _repository.SaveChangesAsync(cancellationToken);

        return new DeleteBuildingComponentResult(buildingComponent);
    }
}
