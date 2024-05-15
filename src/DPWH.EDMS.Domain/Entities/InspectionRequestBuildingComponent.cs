using System.ComponentModel.DataAnnotations.Schema;
using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class InspectionRequestBuildingComponent : EntityBase
{
    private InspectionRequestBuildingComponent()
    {
    }

    private InspectionRequestBuildingComponent(
        InspectionRequest inspectionRequest,
        string mainCategory,
        string subCategory,
        bool forRepair,
        int? rating,
        string? particular)
    {
        InspectionRequestId = inspectionRequest.Id;
        InspectionRequest = inspectionRequest;
        Category = mainCategory;
        SubCategory = subCategory;
        ForRepair = forRepair;
        Rating = rating;
        Particular = particular;
    }

    public static InspectionRequestBuildingComponent Create(
        InspectionRequest inspectionRequest,
        string mainCategory,
        string subCategory,
        bool forRepair,
        int? rating,
        string? particular,
        string createdBy)
    {
        var entity = new InspectionRequestBuildingComponent(inspectionRequest, mainCategory, subCategory, forRepair, rating, particular);
        entity.IsUpdated = false;
        entity.SetCreated(createdBy);

        return entity;
    }
    public static InspectionRequestBuildingComponent CreateComponents(
    InspectionRequest inspectionRequest,
    string mainCategory,
    string subCategory,
    bool forRepair,
    int? rating,
    string? particular,
    string createdBy)
    {
        var entity = new InspectionRequestBuildingComponent(inspectionRequest, mainCategory, subCategory, forRepair, rating, particular);

        entity.IsUpdated = true;
        entity.SetCreated(createdBy);

        return entity;
    }

    public void Update(InspectionRequest inspectionRequest, string mainCategory, string subCategory, bool forRepair, int? rating, string? particular, string modifiedBy)
    {
        InspectionRequest = inspectionRequest;
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
    [ForeignKey("InspectionRequestId")]
    public Guid InspectionRequestId { get; set; }
    public InspectionRequest InspectionRequest { get; set; }
    public virtual InspectionRequestBuildingComponentImage Images { get; set; }
}
