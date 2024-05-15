using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateInspectionRequest;

public record UpdateInspectionRequestCommand : IRequest<UpdateInspectionRequestResult>
{
    public Guid Id { get; set; }
    public bool IsDraft { get; set; }
    public required string Purpose { get; set; }
    public string Status { get; set; }
    public required DateTimeOffset Schedule { get; set; }
    public required DateTimeOffset Deadline { get; set; }
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required IEnumerable<UpdateBuildingComponentCategory> BuildingComponents { get; set; }
    public class UpdateBuildingComponentCategory
    {
        public string Category { get; set; }
        public string? Subcategory { get; set; }
    }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? FurtherInstructions { get; set; }

    //For rental Rates 
    public string PropertyName { get; set; }
    public string? TypeOfStructure { get; set; }
    public string? ImplementingOffice { get; set; }
    public string? Group { get; set; }
    public string? Agency { get; set; }
    public string? AttachedAgency { get; set; }
    public string? Region { get; set; }
    public string? RegionId { get; set; }
    public string? Province { get; set; }
    public string? ProvinceId { get; set; }
    public string? City { get; set; }
    public string? CityId { get; set; }
    public string? StreetAddress { get; set; }
    public double? LongDegrees { get; set; }
    public double? LongMinutes { get; set; }
    public double? LongSeconds { get; set; }
    public string? LongDirection { get; set; }
    public double? LatDegrees { get; set; }
    public double? LatMinutes { get; set; }
    public double? LatSeconds { get; set; }
    public string? LatDirection { get; set; }
}
internal sealed class UpdateInspectionRequestHandler : IRequestHandler<UpdateInspectionRequestCommand, UpdateInspectionRequestResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateInspectionRequestHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateInspectionRequestResult> Handle(UpdateInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        RentalRateProperty rentalRateProperty = null;

        var entity = await _repository.InspectionRequests
            .Include(i => i.InspectionRequestBuildingComponents)
            .Include(i => i.RentalRateProperty)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        var inspectionRequestBuildingComponents = await _repository.InspectionRequestBuildingComponents
            .Where(x => x.InspectionRequestId == request.Id)
            .ToListAsync(cancellationToken);

        var buildingComponents = await _repository
            .BuildingComponents
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var status = (InspectionRequestStatus)Enum.Parse(typeof(InspectionRequestStatus), request.Status);
        if (request.IsDraft) status = InspectionRequestStatus.Ongoing;

        if (entity == null)
        {
            throw new AppException($"Inspection request `{request.Id}` not found");
        }

        if (request.Purpose == "RentalRates" || request.Purpose == "Rental Rates")
        {
            entity.RentalRateProperty?.Update(request.PropertyName, request.TypeOfStructure, request.ImplementingOffice, request.Group, request.Agency, request.AttachedAgency, request.Region, request.RegionId, request.Province, request.ProvinceId, request.City, request.CityId, request.StreetAddress, request.LongDegrees, request.LongMinutes, request.LongSeconds, request.LongDirection, request.LatDegrees, request.LatMinutes, request.LatSeconds, request.LatDirection, _principal.GetUserName());
        }

        entity.UpdateDetails(status, request.Purpose, request.Schedule, request.Deadline, request.EmployeeId, request.EmployeeName, request.PhotosPerArea, request.IsPhotosRequired, request.FurtherInstructions, _principal.GetUserName());

        foreach (var name in request.BuildingComponents)
        {
            var buildingComponentToUpdate = buildingComponents.First(bc => bc.Name == name.Category || bc.Name == name.Subcategory);

            // Check if there's an existing InspectionRequestBuildingComponent with the specified InspectionRequestId and Category name
            var existingComponent = inspectionRequestBuildingComponents
                .FirstOrDefault(ic => ic.InspectionRequestId == request.Id && ic.Category == name.Category || ic.SubCategory == name.Subcategory);

            if (existingComponent != null)
            {
                existingComponent.Update(
                    entity,
                    name.Category,
                    name.Subcategory != null ? buildingComponents.First(bc => bc.Name == name.Subcategory).Name : null,
                    false,
                    0,
                    null,
                    _principal.GetUserName());
            }
            else
            {
                var newComponent = InspectionRequestBuildingComponent.Create(
                    entity,
                    name.Category,
                    name.Subcategory != null ? buildingComponents.First(bc => bc.Name == name.Subcategory).Name : null,
                    false,
                    0,
                    null,
                    _principal.GetUserName());

                _repository.InspectionRequestBuildingComponents.Add(newComponent);
            }
        }
        // Remove any remaining InspectionRequestBuildingComponents with the same InspectionRequestId and different Category name
        var componentsToRemove = inspectionRequestBuildingComponents
            .Where(ic => ic.InspectionRequestId == request.Id && !request.BuildingComponents.Any(bc => bc.Category == ic.Category))
            .ToList();

        _repository.InspectionRequestBuildingComponents.RemoveRange(componentsToRemove);
        _repository.InspectionRequests.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateInspectionRequestResult(entity);
    }
}
