namespace DPWH.EDMS.Application;

public record class UpdateRecordTypeModel(
    string Name,
    string Category,
    string Section,
    string Office,
    bool IsActive
);
