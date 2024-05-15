using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;
public class InspectionRequestProjectMonitoring : EntityBase
{
    private InspectionRequestProjectMonitoring() { }
    private InspectionRequestProjectMonitoring(InspectionRequest inspectionRequest, int? year, int? month, decimal? revisedContractCost, DateTimeOffset? revisedExpiryDate, int? totalDuration, decimal? disbursement, decimal? balance, decimal? financialPlanned, decimal? financialActual, decimal? financialRevised, decimal? physicalPlanned, decimal? physicalActual, decimal? physicalRevised, decimal? physicalSlippage)
    {
        InspectionRequestId = inspectionRequest.Id;
        Year = year;
        Month = month;
        RevisedContractCost = revisedContractCost;
        RevisedExpiryDate = revisedExpiryDate;
        TotalProjectDuration = totalDuration;
        Disbursement = disbursement;
        Balance = balance;
        FinancialPlanned = financialPlanned;
        FinancialActual = financialActual;
        FinancialRevised = financialRevised;
        PhysicalPlanned = physicalPlanned;
        PhysicalActual = physicalActual;
        PhysicalRevised = physicalRevised;
        PhysicalSlippage = physicalSlippage;
    }

    public static InspectionRequestProjectMonitoring Create(InspectionRequest inspectionRequest, int? year, int? month, decimal? revisedContractCost, DateTimeOffset? revisedExpiryDate, int? totalDuration, decimal? disbursement, decimal? balance, decimal? financialPlanned, decimal? financialActual, decimal? financialRevised, decimal? physicalPlanned, decimal? physicalActual, decimal? physicalRevised, decimal? physicalSlippage, string createdBy)
    {
        var entity = new InspectionRequestProjectMonitoring(inspectionRequest, year, month, revisedContractCost, revisedExpiryDate, totalDuration, disbursement, balance, financialPlanned, financialActual, financialRevised, physicalPlanned, physicalActual, physicalRevised, physicalSlippage);

        entity.SetCreated(createdBy);
        return entity;
    }

    public void UpdateDetails(int? year, int? month, decimal? revisedContractCost, DateTimeOffset? revisedExpiryDate, int? totalDuration, decimal? disbursement, decimal? balance, decimal? financialPlanned, decimal? financialActual, decimal? financialRevised, decimal? physicalPlanned, decimal? physicalActual, decimal? physicalRevised, decimal? physicalSlippage, string modifiedBy)
    {
        Year = year;
        Month = month;
        RevisedContractCost = revisedContractCost;
        RevisedExpiryDate = revisedExpiryDate;
        TotalProjectDuration = totalDuration;
        Disbursement = disbursement;
        Balance = balance;
        FinancialPlanned = financialPlanned;
        FinancialActual = financialActual;
        FinancialRevised = financialRevised;
        PhysicalPlanned = physicalPlanned;
        PhysicalActual = physicalActual;
        PhysicalRevised = physicalRevised;
        PhysicalSlippage = physicalSlippage;

        SetModified(modifiedBy);
    }


    //[ForeignKey("InspectionRequestId")]
    public Guid InspectionRequestId { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public decimal? RevisedContractCost { get; set; }
    public DateTimeOffset? RevisedExpiryDate { get; set; }
    public int? TotalProjectDuration { get; set; }
    public decimal? Disbursement { get; set; }
    public decimal? Balance { get; set; }
    public decimal? FinancialPlanned { get; set; }
    public decimal? FinancialActual { get; set; }
    public decimal? FinancialRevised { get; set; }
    public decimal? PhysicalPlanned { get; set; }
    public decimal? PhysicalActual { get; set; }
    public decimal? PhysicalRevised { get; set; }
    public decimal? PhysicalSlippage { get; set; }
    public virtual IList<InspectionRequestProjectMonitoringScope> InspectionRequestProjectMonitoringScopes { get; set; }
    public InspectionRequest? InspectionRequest { get; set; }
}
