using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application;

public record class CreateRecordTypeModel(Guid DataLibraryId, string Division, string Section);
