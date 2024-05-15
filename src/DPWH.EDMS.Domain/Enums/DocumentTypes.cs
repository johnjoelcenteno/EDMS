using System.ComponentModel;

namespace DPWH.EDMS.Domain.Enums;

public enum AssetDocumentCategory
{
    Image,
    File
}

public enum AssetStatus
{
    Draft,
    Submitted,
    Active,
    Archive,
    [Description("For Approval")]
    ForApproval
}

public enum AssetImageView
{
    Front,
    Left,
    Right,
    Back,
    Top
}

public enum MaintenanceDocumentType
{
    [Description("Proof of Ownership")]
    ProofOfOwnership,
    [Description("Programs of Works with Detailed Unit Price Analysis")]
    ProgramOfWorks,
    [Description("Detailed Estimate")]
    DetailedEstimate,
    [Description("Other Documents")]
    OtherDocument
}
