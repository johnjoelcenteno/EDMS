using System.ComponentModel;

namespace DPWH.EDMS.Application.Features.RecordTypes;

public enum RecordTypesCategory
{    
    [Description("DPWH Issuances")]
    Issuances,
    [Description("Employee Records")]
    EmployeeRecords,
    [Description("Employee Documents")]
    EmployeeDocuments
}