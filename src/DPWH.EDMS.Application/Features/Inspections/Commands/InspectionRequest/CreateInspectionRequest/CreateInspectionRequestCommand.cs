using DPWH.EDMS.Domain.Entities;
using InspectionRequestEntity = DPWH.EDMS.Domain.Entities.InspectionRequest;
using MediatR;
using System.Security.Claims;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequest;

public record CreateInspectionRequestCommand : IRequest<Guid>
{
    public bool IsDraft { get; set; }
    public required Guid AssetId { get; set; }
    public required string Purpose { get; set; }
    public string Status { get; set; }
    public required DateTimeOffset Schedule { get; set; }
    public required DateTimeOffset Deadline { get; set; }
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required IEnumerable<BuildingComponentCategory> BuildingComponents { get; set; }
    public class BuildingComponentCategory
    {
        public string Category { get; set; }
        public string? Subcategory { get; set; }
    }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? FurtherInstructions { get; set; }
    public string ContractId { get; set; }
    public string RequestNumber { get; set; }
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
    //public string? Barangay { get; set; }
    //public string? BarangayId { get; set; }
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

internal sealed class CreateInspectionRequestHandler : IRequestHandler<CreateInspectionRequestCommand, Guid>
{
    private readonly ILogger _logger;
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private readonly IRentalRateNumberGeneratorService _generatorService;


    public CreateInspectionRequestHandler(ILogger<CreateInspectionRequestHandler> logger, IWriteRepository repository, ClaimsPrincipal principal, IRentalRateNumberGeneratorService generatorService)
    {
        _logger = logger;
        _repository = repository;
        _principal = principal;
        _generatorService = generatorService;
    }

    public async Task<Guid> Handle(CreateInspectionRequestCommand request, CancellationToken cancellationToken)
    {
        var asset = await _repository.Assets.SingleOrDefaultAsync(a => a.Id == request.AssetId, cancellationToken);
        var status = (InspectionRequestStatus)Enum.Parse(typeof(InspectionRequestStatus), request.Status);

        RentalRateProperty rentalRateProperty = null;
        ProjectMonitoring projectMonitoring = null;
        MaintenanceRequest maintenanceRequest = null;

        if (asset is null && !(request.Purpose.Equals("RentalRates", StringComparison.OrdinalIgnoreCase) || request.Purpose.Equals("Rental Rates", StringComparison.OrdinalIgnoreCase)))
        {
            _logger.LogError("Asset `{AssetId}` not found", request.AssetId);
            throw new AppException("Asset not found");
        }

        switch (request.Purpose)
        {
            case "Priority List Inspection":
                maintenanceRequest = await _repository.MaintenanceRequests.FirstOrDefaultAsync(x => x.RequestNumber == request.RequestNumber, cancellationToken)
                    ?? throw new AppException("No available Maintenance Request");
                break;

            case "Project Monitoring":
                projectMonitoring = await _repository.ProjectMonitoring.Include(x => x.ProjectMonitoringBuildingComponents).FirstOrDefaultAsync(x => x.ContractId == request.ContractId, cancellationToken)
                        ?? throw new AppException("No available Project Monitoring");

                projectMonitoring.UpdateStatus(ProjectMonitoringRequestStatus.Monitoring, _principal.GetUserName());
                _repository.ProjectMonitoring.Update(projectMonitoring);
                break;
        }

        var buildingComponents = await _repository
            .BuildingComponents
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (request.Purpose == "RentalRates" || request.Purpose == "Rental Rates")
        {
            var currentYear = DateTimeOffset.Now;
            var rentalRateNumber = await _generatorService.Generate(currentYear, cancellationToken);

            rentalRateProperty = RentalRateProperty.Create(rentalRateNumber, request.PropertyName, request.TypeOfStructure, request.ImplementingOffice, request.Group, request.Agency, request.AttachedAgency, request.Region, request.RegionId, request.Province, request.ProvinceId, request.City, request.CityId, request.StreetAddress, request.LongDegrees, request.LongMinutes, request.LongSeconds, request.LongDirection, request.LatDegrees, request.LatMinutes, request.LatSeconds, request.LatDirection, _principal.GetUserName());

            await _repository.RentalRateProperty.AddAsync(rentalRateProperty, cancellationToken);
        }


        if (request.IsDraft) status = InspectionRequestStatus.Ongoing;

        var entity = InspectionRequestEntity.Create(
            asset,
            rentalRateProperty,
            request.Purpose,
            status,
            request.Schedule,
            request.Deadline,
            request.EmployeeId,
            request.EmployeeName,
            request.PhotosPerArea,
            request.IsPhotosRequired,
            request.FurtherInstructions,
            projectMonitoring,
            maintenanceRequest,
            _principal.GetUserName());

        entity.InspectionRequestBuildingComponents = request
            .BuildingComponents
            .Select(c => InspectionRequestBuildingComponent.Create(
                entity,
                buildingComponents.First(bc => bc.Name == c.Category).Name,
                c.Subcategory != null ? buildingComponents.First(bc => bc.Name == c.Subcategory).Name : null,
                false,
                0,
                null,
                _principal.GetUserName()))
            .ToList();

        await _repository.InspectionRequests.AddAsync(entity, cancellationToken);

        if (request.Purpose == "Project Monitoring")
        {
            var inspectionRequestProjectMonitoring = InspectionRequestProjectMonitoring.Create(entity, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, _principal.GetUserName());

            var inspectionRequestProjectMonitoringScopes = _repository.InspectionRequestProjectMonitoringScopes
                .Where(x => x.InspectionRequestProjectMonitoringId == null && x.ProjectMonitoringId == projectMonitoring.Id)
                .Include(x => x.ProjectMonitoring)
                .ToList();

            if (inspectionRequestProjectMonitoringScopes.Count == 0) //Check here if there is already a IRPMS created from Create Project Monitoring
            {
                inspectionRequestProjectMonitoring.InspectionRequestProjectMonitoringScopes = projectMonitoring.ProjectMonitoringBuildingComponents
                    .Select(component => InspectionRequestProjectMonitoringScope.Create(
                        inspectionRequestProjectMonitoring,
                        projectMonitoring,
                        component.Category,
                        component.SubCategory,
                        component.ItemNo,
                        component.Description,
                        component.Total,
                        component.Quantity,
                        component.Unit,
                        component.UnitCost,
                        component.TotalCost,
                        null, null, null, null, null, null, null, null, null, null,
                        _principal.GetUserName()))
                    .ToList();
                await _repository.InspectionRequestProjectMonitoring.AddAsync(inspectionRequestProjectMonitoring, cancellationToken);
            }
            else
            {
                await _repository.InspectionRequestProjectMonitoring.AddAsync(inspectionRequestProjectMonitoring, cancellationToken);
                foreach (var scope in inspectionRequestProjectMonitoringScopes)
                {
                    scope.Update(inspectionRequestProjectMonitoring);
                    _repository.InspectionRequestProjectMonitoringScopes.Update(scope);
                }
            }
        }
        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
