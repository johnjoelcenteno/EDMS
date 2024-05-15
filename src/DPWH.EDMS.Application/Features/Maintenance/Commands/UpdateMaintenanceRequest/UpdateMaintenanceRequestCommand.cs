using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.UpdateMaintenanceRequest;
public record UpdateMaintenanceRequestCommand : IRequest<UpdateMaintenanceRequestResult>
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public string? Purpose { get; set; }
    public IEnumerable<string> BuildingComponents { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? FurtherInstructions { get; set; }
    public decimal RequestedAmount { get; set; }
    public string PurposeProjectName { get; set; }
}

internal sealed class UpdateMaintenanceRequestHandler : IRequestHandler<UpdateMaintenanceRequestCommand, UpdateMaintenanceRequestResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateMaintenanceRequestHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateMaintenanceRequestResult> Handle(UpdateMaintenanceRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.MaintenanceRequests
            .Include(i => i.MaintenanceRequestBuildingComponents)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        var maintenanceRequestBuildingComponents = await _repository.MaintenanceRequestBuildingComponents
            .Where(x => x.MaintenanceRequestId == request.Id)
            .ToListAsync(cancellationToken);

        var buildingComponents = await _repository
            .BuildingComponents
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (entity == null)
        {
            throw new AppException($"Maintenance request `{request.Id}` not found");
        }
        var status = (MaintenanceRequestStatus)Enum.Parse(typeof(MaintenanceRequestStatus), request.Status);

        entity.UpdateDetails(
            status,
            request.Purpose,
            request.PhotosPerArea,
            request.IsPhotosRequired,
            request.FurtherInstructions,
            request.RequestedAmount,
            request.PurposeProjectName,
            _principal.GetUserName());

        foreach (var name in request.BuildingComponents)
        {
            var buildingComponentToUpdate = buildingComponents.First(bc => bc.Name == name);

            // Check if there's an existing InspectionRequestBuildingComponent with the specified InspectionRequestId and Category name
            var existingComponent = maintenanceRequestBuildingComponents
                .FirstOrDefault(ic => ic.MaintenanceRequestId == request.Id && ic.Category == name);

            if (existingComponent != null)
            {
                existingComponent.Update(
                    entity,
                    buildingComponentToUpdate.Name,
                    null,
                    false,
                    0,
                    null,
                    _principal.GetUserName());
            }
            else
            {
                var newComponent = MaintenanceRequestBuildingComponent.Create(
                    entity,
                    buildingComponentToUpdate.Name,
                    null,
                    false,
                    0,
                    null,
                    _principal.GetUserName());

                _repository.MaintenanceRequestBuildingComponents.Add(newComponent);
            }
        }
        // Remove any remaining InspectionRequestBuildingComponents with the same InspectionRequestId and different Category name
        var componentsToRemove = maintenanceRequestBuildingComponents
            .Where(ic => ic.MaintenanceRequestId == request.Id && !request.BuildingComponents.Contains(ic.Category))
            .ToList();

        _repository.MaintenanceRequestBuildingComponents.RemoveRange(componentsToRemove);
        _repository.MaintenanceRequests.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateMaintenanceRequestResult(entity);
    }
}
