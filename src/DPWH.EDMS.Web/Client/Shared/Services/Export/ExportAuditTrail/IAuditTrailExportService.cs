namespace DPWH.EDMS.Web.Client.Shared.Services.Export.ExportAuditTrail;

public interface IAuditTrailExportService
{
    Task ExportList<T>(bool IsInventory, List<T> dataList, string filename = "list.xlsx");
}