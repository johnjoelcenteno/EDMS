using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class ProjectMonitoring : EntityBase
{
    private ProjectMonitoring() { }

    private ProjectMonitoring(Guid assetId, MaintenanceRequest maintenanceRequest, string contractId, string projectName, string status, DateTimeOffset saaDate, string saaNumber, decimal? allocation, decimal? contractCost, decimal? revisedContractCost, DateTimeOffset startDate, DateTimeOffset expiryDate, DateTimeOffset? revisedExpiryDate, string projectDuration, int? totalProjectDuration, string remarks)
    {
        AssetId = assetId;
        MaintenanceRequestNumber = maintenanceRequest.RequestNumber;
        ContractId = contractId;
        ProjectName = projectName;
        Status = status;
        SAADate = saaDate;
        SAANumber = saaNumber;
        Allocation = allocation;
        ContractCost = contractCost;
        RevisedContractCost = revisedContractCost;
        StartDate = startDate;
        ExpiryDate = expiryDate;
        RevisedExpiryDate = revisedExpiryDate;
        ProjectDuration = projectDuration;
        TotalProjectDuration = totalProjectDuration;
        Remarks = remarks;
    }

    public static ProjectMonitoring Create(Guid assetId, MaintenanceRequest maintenanceRequest, string contractId, string projectName, string status, DateTimeOffset saaDate, string saaNumber, decimal? allocation, decimal? contractCost, decimal? revisedContractCost, DateTimeOffset startDate, DateTimeOffset expiryDate, DateTimeOffset? revisedExpiryDate, string projectDuration, int? totalProjectDuration, string remarks, string createdBy)
    {
        var entity = new ProjectMonitoring(assetId, maintenanceRequest, contractId, projectName, status, saaDate, saaNumber, allocation, contractCost, revisedContractCost, startDate, expiryDate, revisedExpiryDate, projectDuration, totalProjectDuration, remarks);

        entity.SetCreated(createdBy);
        return entity;
    }

    public void UpdateDetails(string contractId, string projectName, string status, DateTimeOffset saaDate, string saaNumber, decimal? allocation, decimal? contractCost, decimal? revisedContractCost, DateTimeOffset startDate, DateTimeOffset expiryDate, DateTimeOffset revisedExpiryDate, string projectDuration, int? totalProjectDuration, string remarks, string modifiedBy)
    {
        ContractId = contractId;
        ProjectName = projectName;
        Status = status;
        SAADate = saaDate;
        SAANumber = saaNumber;
        Allocation = allocation;
        ContractCost = contractCost;
        RevisedContractCost = revisedContractCost;
        StartDate = startDate;
        ExpiryDate = expiryDate;
        RevisedExpiryDate = revisedExpiryDate;
        ProjectDuration = projectDuration;
        TotalProjectDuration = totalProjectDuration;
        Remarks = remarks;

        SetModified(modifiedBy);
    }

    public void Update(int? month, int? year, decimal? revisedContractCost, DateTimeOffset? revisedExpiryDate, int? totalProjectDuration, decimal? disbursement, decimal? balance, decimal? financialPlanned, decimal? financialRevised, decimal? financialActual, decimal? physicalPlanned, decimal? physicalRevised, decimal? physicalActual, decimal? physicalSlippage, string modifiedBy)
    {
        Month = month;
        Year = year;
        RevisedContractCost = revisedContractCost;
        RevisedExpiryDate = revisedExpiryDate;
        TotalProjectDuration = totalProjectDuration;
        Disbursement = disbursement;
        Balance = balance;
        FinancialPlanned = financialPlanned;
        FinancialActual = financialActual;
        FinancialRevised = financialRevised;
        PhysicalPlanned = physicalPlanned;
        PhysicalRevised = physicalRevised;
        PhysicalActual = physicalActual;
        PhysicalSlippage = physicalSlippage;

        SetModified(modifiedBy);
    }
    public void UpdateStatus(ProjectMonitoringRequestStatus status, string modifiedBy)
    {
        Status = status.ToString();
        SetModified(modifiedBy);
    }

    public Guid AssetId { get; set; }
    public Asset Asset { get; set; }
    public string MaintenanceRequestNumber { get; set; }
    public string ContractId { get; set; }
    public string ProjectName { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
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
    public string? Remarks { get; set; }
    public virtual IList<ProjectMonitoringScope>? ProjectMonitoringBuildingComponents { get; set; }
    public IList<InspectionRequestProjectMonitoringScope> InspectionRequestProjectMonitoringScopes { get; set; }
}
