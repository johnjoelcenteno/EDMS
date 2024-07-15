using System.ComponentModel;

namespace DPWH.EDMS.Domain.Enums;

public enum RecordTypesCategory
{
    [Description("DPWH Issuances")]
    Issuances,
    [Description("Personal Records")]
    PersonalRecords,
    [Description("Employee Documents")]
    EmployeeDocuments
}
