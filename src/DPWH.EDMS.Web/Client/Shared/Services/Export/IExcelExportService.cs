namespace DPWH.EDMS.Web.Client.Shared.Services.Export;

public interface IExcelExportService
{
    Task ExportList<T>(List<T> dataList, string filename = "list.xlsx");
}
