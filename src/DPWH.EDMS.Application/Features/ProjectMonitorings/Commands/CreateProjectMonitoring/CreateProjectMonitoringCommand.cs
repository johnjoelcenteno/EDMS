using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoring;

public record CreateProjectMonitoringCommand : IRequest<Guid>
{
    public bool IsDraft { get; set; }
    public Guid AssetId { get; set; }
    public string ContractId { get; set; }
    public string MaintenanceRequestNumber { get; set; }
    public string ProjectName { get; set; }
    public string Status { get; set; }
    public DateTimeOffset SAADate { get; set; }
    public string SAANumber { get; set; }
    public decimal? Allocation { get; set; }
    public decimal? ContractCost { get; set; }
    public decimal? RevisedContractCost { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public DateTimeOffset? RevisedExpiryDate { get; set; }
    public string ProjectDuration { get; set; }
    public int TotalProjectDuration { get; set; }
    public string? Remarks { get; set; }
    public List<Component> Components { get; set; }
    public class Component
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public decimal? Total { get; set; }
        public string? Quantity { get; set; }
        public string? Unit { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
internal sealed class CreateProjectMonitoringCommandHandler : IRequestHandler<CreateProjectMonitoringCommand, Guid>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateProjectMonitoringCommandHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<Guid> Handle(CreateProjectMonitoringCommand request, CancellationToken cancellationToken)
    {
        var asset = await _repository.Assets.SingleOrDefaultAsync(a => a.Id == request.AssetId, cancellationToken) ?? throw new AppException("Asset not found");

        var maintenanceRequest = await _repository.MaintenanceRequests.FirstOrDefaultAsync(a => a.RequestNumber == request.MaintenanceRequestNumber, cancellationToken)
                ?? throw new AppException("Maintenance request not found");

        var existingEntityWithContractId = await _repository.ProjectMonitoring
            .FirstOrDefaultAsync(p => p.ContractId == request.ContractId, cancellationToken);

        if (existingEntityWithContractId != null)
        {
            throw new AppException($"A project monitoring with the contractId '{request.ContractId}' already exists.");
        }

        var buildingComponents = await _repository
         .BuildingComponents
         .AsNoTracking()
         .ToListAsync(cancellationToken);

        var entity = ProjectMonitoring.Create(asset.Id, maintenanceRequest, request.ContractId, request.ProjectName, request.Status, request.SAADate, request.SAANumber, request.Allocation, request.ContractCost, request.RevisedContractCost, request.StartDate, request.ExpiryDate, request.RevisedExpiryDate, request.ProjectDuration, request.TotalProjectDuration, request.Remarks, _principal.GetUserName());

        entity.ProjectMonitoringBuildingComponents = request.Components
            .Select(component => ProjectMonitoringScope.Create(
                entity,
                component.Category,
                component.SubCategory,
                component.ItemNo,
                component.Description,
                component.Total,
                component.Quantity,
                component.Unit,
                component.UnitCost,
                component.TotalCost,
                _principal.GetUserName()
            ))
            .ToList();

        if (request.IsDraft) entity.UpdateStatus(ProjectMonitoringRequestStatus.Ongoing, _principal.GetUserName());

        await _repository.ProjectMonitoring.AddAsync(entity, cancellationToken);

        // Create IRPMS here
        var irProjectMonitoringScope = entity.ProjectMonitoringBuildingComponents
           .Select(component => InspectionRequestProjectMonitoringScope.Create(
               null,
               entity,
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
               _principal.GetUserName()));

        await _repository.InspectionRequestProjectMonitoringScopes.AddRangeAsync(irProjectMonitoringScope, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
