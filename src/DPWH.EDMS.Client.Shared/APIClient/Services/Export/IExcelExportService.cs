using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Export;

public interface IExcelExportService
{
    Task ExportList<T>(List<T> dataList, string filename = "list.xlsx");
}
