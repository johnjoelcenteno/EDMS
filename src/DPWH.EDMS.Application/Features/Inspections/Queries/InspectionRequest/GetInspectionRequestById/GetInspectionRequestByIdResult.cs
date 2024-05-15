using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequests;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequestById;

public record GetInspectionRequestByIdResult
{
    public GetInspectionRequestByIdResult(InspectionRequestModel model)
    {
        Id = model.Id;
        AssetId = model.AssetId;
        RentalRateId = model.RentalRateId;
        BuildingId = model.BuildingId;
        Status = model.Status;
        Purpose = model.Purpose;
        Schedule = model.Schedule;
        Deadline = model.Deadline;
        EmployeeId = model.EmployeeId;
        EmployeeName = model.EmployeeName;
        PhotosPerArea = model.PhotosPerArea;
        IsPhotosRequired = model.IsPhotosRequired;
        Instructions = model.FurtherInstructions;
        BuildingComponents = model.BuildingComponents
            .GroupBy(c => c.Category)
            .SelectMany(g => g.Select((c, index) => new InspectionRequestBuildingComponentsModel
            {
                Id = c.Id,
                Category = g.Key,
                Status = g.First().Status,
                SubCategories = c.SubCategories
            }))
            .ToList();
        Documents = new InspectionRequestDocumentModel
        {
            FileName = model.Documents.FileName,
            Name = model.Documents.Name,
            Uri = model.Documents.Uri
        };
        ProjectMonitorings = new InspectionProjectMonitoringModel
        {
            Id = model.ProjectMonitorings.Id,
            ContractId = model.ProjectMonitorings.ContractId,
            ProjectName = model.ProjectMonitorings.ProjectName,
            ContractCost = model.ProjectMonitorings.ContractCost,
            RevisedContractCost = model.ProjectMonitorings.RevisedContractCost,
            RevisedExpiryDate = model.ProjectMonitorings.RevisedExpiryDate,
            TotalProjectDuration = model.ProjectMonitorings.TotalProjectDuration,
            Disbursement = model.ProjectMonitorings.Disbursement,
            Balance = model.ProjectMonitorings.Balance,
            FinancialPlanned = model.ProjectMonitorings.FinancialPlanned,
            FinancialActual = model.ProjectMonitorings.FinancialActual,
            FinancialRevised = model.ProjectMonitorings.FinancialRevised,
            PhysicalPlanned = model.ProjectMonitorings.PhysicalPlanned,
            PhysicalActual = model.ProjectMonitorings.PhysicalActual,
            PhysicalRevised = model.ProjectMonitorings.PhysicalRevised,
            PhysicalSlippage = model.ProjectMonitorings.PhysicalSlippage,
            ProjectMonitoringBuildingComponents = model.ProjectMonitorings.ProjectMonitoringBuildingComponents
                                .GroupBy(c => c.Category)
                                .Select(group => new ProjectMonitoringBuildingComponentsModel
                                {
                                    Category = group.Key,
                                    Subcategories = group
                                        .SelectMany(c => c.Subcategories)
                                        .GroupBy(sub => sub.Subcategory)
                                        .Select(subGroup => new ProjectMonitoringSubcategoryModel
                                        {
                                            Subcategory = subGroup.Key,
                                            Items = subGroup
                                                .SelectMany(sub => sub.Items)
                                                .Select(item => new ProjectMonitoringItemsModel
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
                                                    Images = item.Images.Select(i => new ProjectMonitoringItemsImageModel
                                                    {
                                                        Id = i.Id,
                                                        Filename = i.Filename,
                                                        Uri = i.Uri,
                                                        FileSize = i.FileSize
                                                    }).ToList()
                                                })
                                        })
                                })
        };
    }
    public Guid Id { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? RentalRateId { get; set; }
    public string BuildingId { get; set; }
    public string Status { get; set; }
    public string Purpose { get; set; }
    public DateTimeOffset? Schedule { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public string? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? Instructions { get; set; }
    public IEnumerable<InspectionRequestBuildingComponentsModel> BuildingComponents { get; set; }
    public InspectionRequestDocumentModel Documents { get; set; }
    public InspectionProjectMonitoringModel ProjectMonitorings { get; set; }
}
