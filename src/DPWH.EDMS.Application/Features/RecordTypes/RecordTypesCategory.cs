using System.ComponentModel;

namespace DPWH.EDMS.Application.Features.RecordTypes;

public enum RecordTypesCategory
{
    Archived,
    [Description("DPWH Issuances")]
    Issuances,
    [Description("Employee Records")]
    EmployeeRecords
}