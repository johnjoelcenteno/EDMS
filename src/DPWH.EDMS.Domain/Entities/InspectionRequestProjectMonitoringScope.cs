using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public class InspectionRequestProjectMonitoringScope : EntityBase
{
    private InspectionRequestProjectMonitoringScope() { }
    public InspectionRequestProjectMonitoringScope(InspectionRequestProjectMonitoring? inspectionRequestProjectMonitoring, ProjectMonitoring projectMonitoring, string? category, string? subcategory, string? itemNo, string? description, decimal? total, string? quantity, string? unit, decimal? unitCost, decimal? totalCost, decimal? financialPlanned, decimal? financialActual, decimal? financialRevised, decimal? physicalPlanned, decimal? physicalRelativePlanned, decimal? physicalRelativeActual, decimal? physicalActual, decimal? physicalRelativeRevised, decimal? physicalRevised, string? remarks)
    {
        InspectionRequestProjectMonitoringId = inspectionRequestProjectMonitoring?.Id;
        ProjectMonitoringId = projectMonitoring?.Id;
        Category = category;
        Subcategory = subcategory;
        ItemNo = itemNo;
        Description = description;
        Total = total;
        Quantity = quantity;
        Unit = unit;
        UnitCost = unitCost;
        TotalCost = totalCost;
        FinancialPlanned = financialPlanned;
        FinancialActual = financialActual;
        FinancialRevised = financialRevised;
        PhysicalPlanned = physicalPlanned;
        PhysicalRelativePlanned = physicalRelativePlanned;
        PhysicalRelativeActual = physicalRelativeActual;
        PhysicalActual = physicalActual;
        PhysicalRelativeRevised = physicalRelativeRevised;
        PhysicalRevised = physicalRevised;
        Remarks = remarks;
    }

    public static InspectionRequestProjectMonitoringScope Create(InspectionRequestProjectMonitoring? inspectionRequestProjectMonitoring, ProjectMonitoring projectMonitoring, string? category, string? subcategory, string? itemNo, string? description, decimal? total, string? quantity, string? unit, decimal? unitCost, decimal? totalCost, decimal? financialPlanned, decimal? financialActual, decimal? financialRevised, decimal? physicalPlanned, decimal? physicalRelativePlanned, decimal? physicalRelativeActual, decimal? physicalActual, decimal? physicalRelativeRevised, decimal? physicalRevised, string? remarks, string createdBy)
    {
        var entity = new InspectionRequestProjectMonitoringScope(inspectionRequestProjectMonitoring, projectMonitoring, category, subcategory, itemNo, description, total, quantity, unit, unitCost, totalCost, financialPlanned, financialActual, financialRevised, physicalPlanned, physicalRelativePlanned, physicalRelativeActual, physicalActual, physicalRelativeRevised, physicalRevised, remarks);

        entity.SetCreated(createdBy);
        return entity;
    }

    public void Update(decimal? financialPlanned, decimal? financialActual, decimal? financialRevised, decimal? physicalPlanned, decimal? physicalRelativePlanned, decimal? physicalRelativeActual, decimal? physicalActual, decimal? physicalRelativeRevised, decimal? physicalRevised, string? remarks, string modifiedBy)
    {
        FinancialPlanned = financialPlanned;
        FinancialActual = financialActual;
        FinancialRevised = financialRevised;
        PhysicalPlanned = physicalPlanned;
        PhysicalRelativePlanned = physicalRelativePlanned;
        PhysicalRelativeActual = physicalRelativeActual;
        PhysicalActual = physicalActual;
        PhysicalRelativeRevised = physicalRelativeRevised;
        PhysicalRevised = physicalRevised;
        Remarks = remarks;

        SetModified(modifiedBy);
    }

    public void Update(InspectionRequestProjectMonitoring? inspectionRequestProjectMonitoring)
    {
        InspectionRequestProjectMonitoringId = inspectionRequestProjectMonitoring?.Id;
    }

    //[ForeignKey("InspectionRequestProjectMonitoringId")]
    public Guid? InspectionRequestProjectMonitoringId { get; set; }
    public Guid? ProjectMonitoringId { get; set; }
    public ProjectMonitoring ProjectMonitoring { get; set; }
    public string? Category { get; set; }
    public string? Subcategory { get; set; }
    public string? ItemNo { get; set; }
    public string? Description { get; set; }
    public decimal? Total { get; set; }
    public string? Quantity { get; set; }
    public string? Unit { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? TotalCost { get; set; }
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
    public virtual IList<InspectionRequestProjectMonitoringScopesImage> InspectionRequestProjectMonitoringScopesImage { get; set; }
}
