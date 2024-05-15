using System.ComponentModel.DataAnnotations.Schema;
using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class FinancialDetails : EntityBase
{
    public FinancialDetails() { }

    public static FinancialDetails Create(string paymentDetails, string orNumber, DateTimeOffset? paymentDate, decimal amountPaid,
        string policy, string policyNumber, string policyId, DateTimeOffset effectivityStart, string particular, string building, string content,
        decimal? premium, decimal? totalPremium, string remarks, string createdBy, DateTimeOffset effectivityEnd)
    {
        var financialDetail = new FinancialDetails
        {
            PaymentDetails = paymentDetails,
            ORNumber = orNumber,
            PaymentDate = paymentDate,
            AmountPaid = amountPaid,
            Policy = policy,
            PolicyNumber = policyNumber,
            PolicyID = policyId,
            EffectivityStart = effectivityStart,
            EffectivityEnd = effectivityEnd,
            Particular = particular,
            Building = building,
            Content = content,
            Premium = premium,
            TotalPremium = totalPremium,
            Remarks = remarks,
        };

        financialDetail.SetCreated(createdBy);
        return financialDetail;
    }

    public void Update(string paymentDetails, string orNumber, DateTimeOffset? paymentDate, decimal amountPaid,
        string policy, string policyNumber, string policyId, DateTimeOffset effectivityStart, string particular, string building, string content,
        decimal? premium, decimal? totalPremium, string remarks, string modifiedBy, DateTimeOffset effectivityEnd)
    {
        PaymentDetails = paymentDetails;
        ORNumber = orNumber;
        PaymentDate = paymentDate;
        AmountPaid = amountPaid;
        Policy = policy;
        PolicyNumber = policyNumber;
        PolicyID = policyId;
        EffectivityStart = effectivityStart;
        EffectivityEnd = effectivityEnd;
        Particular = particular;
        Building = building;
        Content = content;
        Premium = premium;
        TotalPremium = totalPremium;
        Remarks = remarks;

        SetModified(modifiedBy);
    }

    [ForeignKey("AssetId")]
    public Guid AssetId { get; set; }
    public virtual Asset Asset { get; set; }
    #region Insurance Policy
    public string? PaymentDetails { get; set; }
    public string? ORNumber { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string? Policy { get; set; }
    public string? PolicyNumber { get; set; }
    public string? PolicyID { get; set; }
    public DateTimeOffset EffectivityStart { get; set; }
    public DateTimeOffset EffectivityEnd { get; set; }
    public string? Particular { get; set; }
    public string? Building { get; set; }
    public string? Content { get; set; }
    public decimal? Premium { get; set; }
    public decimal? TotalPremium { get; set; }
    public string? Remarks { get; set; }
    #endregion
}