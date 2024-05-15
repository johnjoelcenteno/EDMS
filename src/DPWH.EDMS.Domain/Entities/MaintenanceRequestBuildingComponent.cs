using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;
public class MaintenanceRequestBuildingComponent : EntityBase
{
    public MaintenanceRequestBuildingComponent()
    {
    }

    private MaintenanceRequestBuildingComponent(
        MaintenanceRequest maintenanceRequest,
        string mainCategory,
        string subCategory,
        bool forRepair,
        int? rating,
        string? particular)
    {
        MaintenanceRequestId = maintenanceRequest.Id;
        MaintenanceRequest = maintenanceRequest;
        Category = mainCategory;
        SubCategory = subCategory;
        ForRepair = forRepair;
        Rating = rating;
        Particular = particular;
    }

    public static MaintenanceRequestBuildingComponent Create(
        MaintenanceRequest maintenanceRequest,
        string mainCategory,
        string subCategory,
        bool forRepair,
        int? rating,
        string? particular,
        string createdBy)
    {
        var entity = new MaintenanceRequestBuildingComponent(maintenanceRequest, mainCategory, subCategory, forRepair, rating, particular);
        entity.SetCreated(createdBy);

        return entity;
    }

    public static MaintenanceRequestBuildingComponent CreateComponents(
        MaintenanceRequest maintenanceRequest,
        string mainCategory,
        string subCategory,
        bool forRepair,
        int? rating,
        string? particular,
        string createdBy)
    {
        var entity = new MaintenanceRequestBuildingComponent(maintenanceRequest, mainCategory, subCategory, forRepair, rating, particular);

        entity.IsUpdated = true;
        entity.SetCreated(createdBy);

        return entity;
    }

    public void Update(MaintenanceRequest maintenanceRequest, string mainCategory, string subCategory, bool forRepair, int? rating, string? particular, string modifiedBy)
    {
        MaintenanceRequest = maintenanceRequest;
        Category = mainCategory;
        SubCategory = subCategory;
        ForRepair = forRepair;
        Rating = rating;
        Particular = particular;

        SetModified(modifiedBy);
    }
    public void UpdateDetails(string subCategory, bool forRepair, int? rating, string? particular, string modifiedBy)
    {
        SubCategory = subCategory;
        ForRepair = forRepair;
        Rating = rating;
        Particular = particular;
        IsUpdated = true;

        SetModified(modifiedBy);
    }

    public string Category { get; set; }
    public string SubCategory { get; set; }
    public bool ForRepair { get; set; }
    public int? Rating { get; set; }
    public bool? IsUpdated { get; set; }
    public string? Particular { get; set; }
    [ForeignKey("MaintenanceRequestId")]
    public Guid MaintenanceRequestId { get; set; }
    public MaintenanceRequest MaintenanceRequest { get; set; }
}
