using ClosedXML.Excel;
using Microsoft.JSInterop;

namespace DPWH.EDMS.Web.Client.Shared.Services.Export;

public class ExcelExportService: IExcelExportService
{
    private readonly IJSRuntime _JsRuntime;
    public ExcelExportService(IJSRuntime jsRuntime)
    {
        _JsRuntime = jsRuntime;
    }

    public async Task ExportList<T>(List<T> dataList, string filename = $"list.xlsx")
    {
        try
        {

            var columnDefinitions = GetColumnDefinitions<T>();
            await ExportData(dataList, columnDefinitions, filename);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
        }
    }

    private List<(string Header, Func<T, object> ValueGetter)> GetColumnDefinitions<T>()
    {
        var properties = typeof(T).GetProperties();

        var columnDefinitions = properties.Select(property =>
            (property.Name, (Func<T, object>)(data => property.GetValue(data)))
        ).ToList();

        return columnDefinitions;
    }

    private async Task ExportData<T>(List<T> data, List<(string Header, Func<T, object> ValueGetter)> columnDefinitions, string fileName)
    {
        if (data != null && data.Count > 0)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("DataSheet");

            // Add headers
            for (int col = 0; col < columnDefinitions.Count; col++)
            {
                worksheet.Cell(1, col + 1).Value = columnDefinitions[col].Header;
            }

            // Populate data
            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];

                for (int col = 0; col < columnDefinitions.Count; col++)
                {
                    var cellValue = columnDefinitions[col].ValueGetter(item);

                    // Cast the cellValue to the appropriate data type
                    if (cellValue is int intValue)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = intValue;
                    }
                    if(cellValue is Guid guidValue)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = guidValue.ToString();
                    }
                    else if (cellValue is string stringValue)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = stringValue;
                    }
                    else if (cellValue is decimal decimalValue)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = decimalValue;
                    }
                    // Add more cases for other data types as needed
                    else
                    {
                        // Handle unsupported data types or null values
                        worksheet.Cell(row + 2, col + 1).Value = string.Empty; // or handle nulls appropriately
                    }
                }
            }

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Trigger a file download from the browser
            await _JsRuntime.InvokeVoidAsync("saveExcelAsFile", fileName, Convert.ToBase64String(memoryStream.ToArray()));
        }
    }
}
