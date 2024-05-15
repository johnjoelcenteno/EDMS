using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateInspectionRequest;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateProjectMonitoring;

public record UpdateInspectionRequestProjectMonitoringCommand : IRequest<UpdateInspectionRequestResult>
{
    public bool IsDraft { get; set; }
    public Guid InspectionRequestId { get; set; }
    public Guid ProjectMonitoringId { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public decimal RevisedContractCost { get; set; }
    public DateTimeOffset RevisedExpiryDate { get; set; }
    public int TotalProjectDuration { get; set; }
    public decimal Disbursement { get; set; }
    public decimal Balance { get; set; }
    public List<ProjectMonitoringComponent> Components { get; set; }
    public class ProjectMonitoringComponent
    {
        public Guid ComponentId { get; set; }
        public decimal? FinancialPlanned { get; set; }
        public decimal? FinancialActual { get; set; }
        public decimal? FinancialRevised { get; set; }
        public decimal? PhysicalRelativePlanned { get; set; }
        public decimal? PhysicalPlanned { get; set; }
        public decimal? PhysicalRelativeActual { get; set; }
        public decimal? PhysicalActual { get; set; }
        public decimal? PhysicalRelativeRevised { get; set; }
        public decimal? PhysicalRevised { get; set; }
        public string? Remarks { get; set; }
    }
    public decimal? FinancialPlanned { get; set; }
    public decimal? FinancialRevised { get; set; }
    public decimal? FinancialActual { get; set; }
    public decimal? PhysicalPlanned { get; set; }
    public decimal? PhysicalRevised { get; set; }
    public decimal? PhysicalActual { get; set; }
    public decimal? PhysicalSlippage { get; set; }
}
internal sealed class UpdateProjectMonitoringHandler : IRequestHandler<UpdateInspectionRequestProjectMonitoringCommand, UpdateInspectionRequestResult>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public UpdateProjectMonitoringHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task<UpdateInspectionRequestResult> Handle(UpdateInspectionRequestProjectMonitoringCommand request, CancellationToken cancellationToken)
    {
        var inspection = _repository.InspectionRequests.FirstOrDefault(x => x.Id == request.InspectionRequestId)
            ?? throw new AppException("No inspection found");

        var projectMonitoring = await _repository.InspectionRequestProjectMonitoring.Include(x => x.InspectionRequestProjectMonitoringScopes).FirstOrDefaultAsync(x => x.Id == request.ProjectMonitoringId)
            ?? throw new AppException("No project monitoring found");

        var projectMonitoringBuildingComponents = _repository.InspectionRequestProjectMonitoringScopes
            .Where(x => x.InspectionRequestProjectMonitoringId == request.ProjectMonitoringId)
            .ToList()
            ?? throw new AppException("No project monitoring categories found");

        projectMonitoring.UpdateDetails(request.Year, request.Month, request.RevisedContractCost, request.RevisedExpiryDate, request.TotalProjectDuration, request.Disbursement, request.Balance, request.FinancialPlanned, request.FinancialActual, request.FinancialRevised, request.PhysicalPlanned, request.PhysicalActual, request.PhysicalRevised, request.PhysicalSlippage, _principal.GetUserName());

        foreach (var field in request.Components)
        {
            var componentToUpdate = projectMonitoringBuildingComponents.FirstOrDefault(c => c.Id == field.ComponentId);

            componentToUpdate?.Update(field.FinancialPlanned, field.FinancialActual, field.FinancialRevised, field.PhysicalPlanned, field.PhysicalRelativePlanned, field.PhysicalRelativeActual, field.PhysicalActual, field.PhysicalRelativeRevised, field.PhysicalRevised, field.Remarks, _principal.GetUserName());
        }

        if (!request.IsDraft)
        {
            var allProjectMonitorings = await _repository.InspectionRequestProjectMonitoring.Include(x => x.InspectionRequestProjectMonitoringScopes).ToListAsync();

            decimal totalFinancialPlanned = 0;
            decimal totalFinancialActual = 0;
            decimal totalFinancialRevised = 0;
            decimal totalPhysicalPlanned = 0;
            decimal totalPhysicalActual = 0;
            decimal totalPhysicalRevised = 0;
            decimal totalPhysicalSlippage = 0;

            // Iterate through all project monitorings
            foreach (var data in allProjectMonitorings)
            {
                totalFinancialPlanned += data.FinancialPlanned ?? 0;
                totalFinancialActual += data.FinancialActual ?? 0;
                totalFinancialRevised += data.FinancialRevised ?? 0;
                totalPhysicalPlanned += data.PhysicalPlanned ?? 0;
                totalPhysicalActual += data.PhysicalActual ?? 0;
                totalPhysicalRevised += data.PhysicalRevised ?? 0;
                totalPhysicalSlippage += data.PhysicalSlippage ?? 0;
            }

            // Get the parent project monitoring
            var parentProjectMonitoring = await _repository.ProjectMonitoring.FirstOrDefaultAsync(x => x.Id == inspection.ProjectMonitoringId);

            // Update parentProjectMonitoring with the total sums
            parentProjectMonitoring.Update(
                request.Month,
                request.Year,
                request.RevisedContractCost,
                request.RevisedExpiryDate,
                request.TotalProjectDuration,
                request.Disbursement,
                request.Balance,
                totalFinancialPlanned,
                totalFinancialRevised,
                totalFinancialActual,
                totalPhysicalPlanned,
                totalPhysicalRevised,
                totalPhysicalActual,
                totalPhysicalSlippage,
                _principal.GetUserName()
            );

            // Save changes to the repository
            inspection.UpdateStatus(InspectionRequestStatus.Submitted, _principal.GetUserName());
            _repository.ProjectMonitoring.Update(parentProjectMonitoring);
        }
        else
        {
            inspection.UpdateStatus(InspectionRequestStatus.Ongoing, _principal.GetUserName());
        }

        _repository.InspectionRequests.Update(inspection);
        _repository.InspectionRequestProjectMonitoring.Update(projectMonitoring);
        _repository.InspectionRequestProjectMonitoringScopes.UpdateRange(projectMonitoringBuildingComponents);
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateInspectionRequestResult(inspection);
    }
}