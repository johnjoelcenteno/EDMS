using DPWH.EDMS.Domain.Entities;
using InspectionRequestEntity = DPWH.EDMS.Domain.Entities.InspectionRequest;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionProjectMonitoringMonthly;
public record GetInspectionProjectMonitoringMonthlyResult
{
    public GetInspectionProjectMonitoringMonthlyResult(ProjectMonitoring? projectMonitoring, List<InspectionRequestEntity>? inspectionRequests)
    {
        ProjectMonitoringId = projectMonitoring?.Id;
        MaintenanceRequestNumber = projectMonitoring?.MaintenanceRequestNumber;
        ContractId = projectMonitoring?.ContractId;
        BuildingId = projectMonitoring?.Asset.BuildingId;
        ProjectName = projectMonitoring?.ProjectName;
        Allocation = projectMonitoring?.Allocation;
        ContractCost = projectMonitoring?.ContractCost;
        Inspections = inspectionRequests?
            .Select(x => new InspectionMonthly
            {
                InspectionRequestId = x.Id,
                InspectorName = x.EmployeeName,
                Month = x.InspectionRequestProjectMonitoring?.Month,
                Year = x.InspectionRequestProjectMonitoring?.Year,
                RevisedContractCost = x.InspectionRequestProjectMonitoring?.RevisedContractCost,
                RevisedExpiryDate = x.InspectionRequestProjectMonitoring?.RevisedExpiryDate,
                TotalProjectDuration = x.InspectionRequestProjectMonitoring?.TotalProjectDuration,
                Disbursement = x.InspectionRequestProjectMonitoring?.Disbursement,
                Balance = x.InspectionRequestProjectMonitoring?.Balance,
                FinancialPlanned = x.InspectionRequestProjectMonitoring?.FinancialPlanned,
                FinancialActual = x.InspectionRequestProjectMonitoring?.FinancialActual,
                FinancialRevised = x.InspectionRequestProjectMonitoring?.FinancialRevised,
                PhysicalPlanned = x.InspectionRequestProjectMonitoring?.PhysicalPlanned,
                PhysicalActual = x.InspectionRequestProjectMonitoring?.PhysicalActual,
                PhysicalRevised = x.InspectionRequestProjectMonitoring?.PhysicalRevised,
                PhysicalSlippage = x.InspectionRequestProjectMonitoring?.PhysicalSlippage,
                InspectionMonthlyComponents = x?.InspectionRequestProjectMonitoring.InspectionRequestProjectMonitoringScopes
                    .GroupBy(c => new { c.Category }) // Group by both Category and Subcategory
                    .Select(c => new InspectionMonthlyComponentsModel
                    {
                        Category = c.Key.Category,
                        Subcategories = c.GroupBy(sub => sub.Subcategory).Select(subCategory => new InspectionMonthlySubComponentsModel
                        {
                            Subcategory = subCategory.Key,
                            Items = subCategory.Select(item => new InspectionMonthlyItemsModel
                            {
                                Id = item.Id,
                                ItemNo = item.ItemNo,
                                Description = item.Description,
                                Total = item.Total,
                                FinancialPlanned = item.FinancialPlanned,
                                FinancialActual = item.FinancialActual,
                                FinancialRevised = item.FinancialRevised,
                                PhysicalPlanned = item.PhysicalPlanned,
                                PhysicalRelativePlanned = item.PhysicalRelativePlanned,
                                PhysicalRelativeActual = item.PhysicalRelativeActual,
                                PhysicalActual = item.PhysicalActual,
                                PhysicalRelativeRevised = item.PhysicalRelativeRevised,
                                PhysicalRevised = item.PhysicalRevised,
                                Remarks = item.Remarks,
                                Images = item.InspectionRequestProjectMonitoringScopesImage?.Select(image => new InspectionMonthlyImagesModel
                                {
                                    Id = image.Id,
                                    Filename = image.Filename,
                                    Uri = image.Uri,
                                    FileSize = image.FileSize
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
            }).ToList();
    }

    public Guid? ProjectMonitoringId { get; set; }
    public string? MaintenanceRequestNumber { get; set; }
    public string? ContractId { get; set; }
    public string? BuildingId { get; set; }
    public string? ProjectName { get; set; }
    public decimal? Allocation { get; set; }
    public decimal? ContractCost { get; set; }

    public List<InspectionMonthly>? Inspections { get; set; }

    public class InspectionMonthly
    {
        public Guid InspectionRequestId { get; set; }
        public string? InspectorName { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? RevisedContractCost { get; set; }
        public DateTimeOffset? RevisedExpiryDate { get; set; }
        public int? TotalProjectDuration { get; set; }
        public decimal? Disbursement { get; set; }
        public decimal? Balance { get; set; }
        public decimal? FinancialPlanned { get; set; }
        public decimal? FinancialRevised { get; set; }
        public decimal? FinancialActual { get; set; }
        public decimal? PhysicalPlanned { get; set; }
        public decimal? PhysicalRevised { get; set; }
        public decimal? PhysicalActual { get; set; }
        public decimal? PhysicalSlippage { get; set; }
        public List<InspectionMonthlyComponentsModel>? InspectionMonthlyComponents { get; set; }
    }

    public class InspectionMonthlyComponentsModel
    {
        public string? Category { get; set; }
        public IEnumerable<InspectionMonthlySubComponentsModel>? Subcategories { get; set; }
    }

    public class InspectionMonthlySubComponentsModel
    {
        public string? Subcategory { get; set; }
        public IEnumerable<InspectionMonthlyItemsModel>? Items { get; set; }
    }

    public class InspectionMonthlyItemsModel
    {
        public Guid Id { get; set; }
        public string? ItemNo { get; set; }
        public string? Description { get; set; }
        public decimal? Total { get; set; }
        public decimal? FinancialPlanned { get; set; }
        public decimal? FinancialActual { get; set; }
        public decimal? FinancialRevised { get; set; }
        public decimal? PhysicalPlanned { get; set; }
        public decimal? PhysicalRelativePlanned { get; set; }
        public decimal? PhysicalRelativeActual { get; set; }
        public decimal? PhysicalActual { get; set; }
        public decimal? PhysicalRelativeRevised { get; set; }
        public decimal? PhysicalRevised { get; set; }
        public string? Remarks { get; set; }
        public List<InspectionMonthlyImagesModel>? Images { get; set; }
    }

    public class InspectionMonthlyImagesModel
    {
        public Guid Id { get; set; }
        public string? Filename { get; set; }
        public string? Uri { get; set; }
        public long? FileSize { get; set; }
    }
}
