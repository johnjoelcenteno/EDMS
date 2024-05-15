using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public class ProjectMonitoringScope : EntityBase
{
    private ProjectMonitoringScope() { }

    private ProjectMonitoringScope(
    ProjectMonitoring projectMonitoring,
    string mainCategory,
    string subCategory,
    string? itemNo,
    string? description,
    decimal? total,
    string? quantity,
    string? unit,
    decimal? unitCost,
    decimal? totalCost)
    {
        ProjectMonitoringId = projectMonitoring.Id;
        Category = mainCategory;
        SubCategory = subCategory;
        ItemNo = itemNo;
        Description = description;
        Total = total;
        Quantity = quantity;
        Unit = unit;
        UnitCost = unitCost;
        TotalCost = totalCost;
    }

    public static ProjectMonitoringScope Create(
        ProjectMonitoring projectMonitoring,
        string mainCategory,
        string subCategory,
        string? itemNo,
        string? description,
        decimal? total,
        string? quantity,
        string? unit,
        decimal? unitCost,
        decimal? totalCost,
        string createdBy)
    {
        var entity = new ProjectMonitoringScope(projectMonitoring, mainCategory, subCategory, itemNo, description, total, quantity, unit, unitCost, totalCost);

        entity.SetCreated(createdBy);
        return entity;
    }

    public void Update(ProjectMonitoring projectMonitoring,
        string mainCategory,
        string subCategory,
        string itemNo,
        string? description,
        decimal? total,
        string? quantity,
        string? unit,
        decimal? unitCost,
        decimal? totalCost,
        string modifiedBy)
    {
        ProjectMonitoringId = projectMonitoring.Id;
        Category = mainCategory;
        SubCategory = subCategory;
        ItemNo = itemNo;
        Description = description;
        Total = total;
        Quantity = quantity;
        Unit = unit;
        UnitCost = unitCost;
        TotalCost = totalCost;

        SetModified(modifiedBy);
    }


    [ForeignKey("ProjectMonitoringId")]
    public Guid ProjectMonitoringId { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public string? ItemNo { get; set; }
    public string? Description { get; set; }
    public decimal? Total { get; set; }
    public string? Quantity { get; set; }
    public string? Unit { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? TotalCost { get; set; }
}
