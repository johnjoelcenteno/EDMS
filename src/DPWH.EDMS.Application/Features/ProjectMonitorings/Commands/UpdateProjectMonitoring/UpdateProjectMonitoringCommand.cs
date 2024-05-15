using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.UpdateProjectMonitoring;

public record UpdateProjectMonitoringCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string ContractId { get; set; }
    public string ProjectName { get; set; }
    public string Status { get; set; }
    public DateTimeOffset SAADate { get; set; }
    public string SAANumber { get; set; }
    public int? Allocation { get; set; }
    public int? ContractCost { get; set; }
    public int? RevisedContractCost { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public DateTimeOffset RevisedExpiryDate { get; set; }
    public string ProjectDuration { get; set; }
    public int? TotalProjectDuration { get; set; }
    public string? Remarks { get; set; }
    public List<UpdateComponent> Components { get; set; }
    public class UpdateComponent
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
internal sealed class UpdateProjectMonitoringHandler : IRequestHandler<UpdateProjectMonitoringCommand, Guid>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateProjectMonitoringHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<Guid> Handle(UpdateProjectMonitoringCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.ProjectMonitoring.Include(i => i.ProjectMonitoringBuildingComponents).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        var projectMonitoringComponents = await _repository.ProjectMonitoringScopes.Where(x => x.ProjectMonitoringId == request.Id).ToListAsync(cancellationToken);

        var buildingComponents = await _repository
            .BuildingComponents
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (entity == null)
        {
            throw new AppException($"Project Monitoring `{request.Id}` not found");
        }

        entity.UpdateDetails(request.ContractId, request.ProjectName, request.Status, request.SAADate, request.SAANumber, request.Allocation, request.ContractCost, request.RevisedContractCost, request.StartDate, request.ExpiryDate, request.RevisedExpiryDate, request.ProjectDuration, request.TotalProjectDuration, request.Remarks, _principal.GetUserName());

        foreach (var name in request.Components)
        {
            var existingComponent = projectMonitoringComponents
                .FirstOrDefault(ic => ic.ProjectMonitoringId == request.Id && ic.Category == name.Category || ic.SubCategory == name.SubCategory);

            if (existingComponent != null)
            {
                existingComponent.Update(
                    entity,
                    name.Category,
                    name.SubCategory,
                    name.ItemNo,
                    name.Description,
                    name.Total,
                    name.Quantity,
                    name.Unit,
                    name.UnitCost,
                    name.TotalCost,
                    _principal.GetUserName());
            }
            else
            {
                var newComponent = ProjectMonitoringScope.Create(
                    entity,
                    name.Category,
                    name.SubCategory,
                    name.ItemNo,
                    name.Description,
                    name.Total,
                    name.Quantity,
                    name.Unit,
                    name.UnitCost,
                    name.TotalCost,
                    _principal.GetUserName());

                _repository.ProjectMonitoringScopes.Add(newComponent);
            }
        }

        var componentsToRemove = projectMonitoringComponents
            .Where(ic => ic.ProjectMonitoringId == request.Id)
            .ToList();
        _repository.ProjectMonitoringScopes.RemoveRange(componentsToRemove);
        _repository.ProjectMonitoring.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
