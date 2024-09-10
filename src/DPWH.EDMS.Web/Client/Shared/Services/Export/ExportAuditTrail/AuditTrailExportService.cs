using ClosedXML.Excel;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.Model;
using Microsoft.JSInterop;

namespace DPWH.EDMS.Web.Client.Shared.Services.Export.ExportAuditTrail;

public class AuditTrailExportService : IAuditTrailExportService
{
    private readonly IJSRuntime _JsRuntime;

    public AuditTrailExportService(IJSRuntime jsRuntime)
    {
        _JsRuntime = jsRuntime;
    }

    public async Task ExportList<T>(bool IsInventory, List<T> dataList, string filename = $"list.xlsx")
    {
        try
        {

            var columnDefinitions = GetColumnDefinitions<T>();
            await ExportData(dataList, columnDefinitions, filename, IsInventory);
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

    private async Task ExportData<T>(List<T> data, List<(string Header, Func<T, object> ValueGetter)> columnDefinitions, string fileName, bool IsInventory)
    {
        if (data != null && data.Count > 0)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("DataSheet");

            int currentRow = 1; // Starting from row 1 for headers

            int startingColumn = 1;

            // Add headers for the main data
            for (int col = 0; col < columnDefinitions.Count; col++)
            {
                var column = columnDefinitions[col];

                // Check if the header is not in the exclusion list
                if (!ShouldExcludeColumn(column.Header, IsInventory))
                {
                    // Rename "TargetUser" header to "UpdatedUser"
                    var header = (column.Header == "TargetUser") ? "Updated User" : column.Header;
                    worksheet.Cell(currentRow, startingColumn).Value = header;
                    startingColumn++;
                }
            }

            // Check if there are any headers for the Changes list
            var changesHeadersAdded = false;

            for (int col = 0; col < columnDefinitions.Count; col++)
            {
                var column = columnDefinitions[col];

                if (column.Header == "Changes" && !changesHeadersAdded)
                {
                    worksheet.Cell(currentRow, startingColumn).Value = "Field";
                    worksheet.Cell(currentRow, startingColumn + 1).Value = "From";
                    worksheet.Cell(currentRow, startingColumn + 2).Value = "To";
                    startingColumn += 3;
                    changesHeadersAdded = true;
                }
            }

            currentRow++; // Move to the next row for data

            // Populate data
            foreach (var item in data)
            {
                // Retrieve the Changes list for the current item
                if (item is AuditLogModel auditLogItem)
                {
                    var changeList = auditLogItem.Changes;

                    // Add rows for each ChangeModel
                    foreach (var change in changeList)
                    {
                        startingColumn = 1; // Reset the starting column for each row

                        // Add the main data row for each change
                        for (int col = 0; col < columnDefinitions.Count; col++)
                        {
                            var column = columnDefinitions[col];

                            // Check if the header is not in the exclusion list
                            if (!ShouldExcludeColumn(column.Header, IsInventory))
                            {
                                var cellValue = column.ValueGetter(item);
                                SetCellValue(worksheet, currentRow, startingColumn, cellValue);
                                startingColumn++;
                            }
                        }

                        // Add columns for changes
                        worksheet.Cell(currentRow, startingColumn).Value = change.Field;
                        worksheet.Cell(currentRow, startingColumn + 1).Value = change.From;
                        worksheet.Cell(currentRow, startingColumn + 2).Value = change.To;

                        // Move to the next row
                        currentRow++;
                    }

                    // Add an empty row after each set of results
                    currentRow++;
                }
            }

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Trigger a file download from the browser
            await _JsRuntime.InvokeVoidAsync("saveExcelAsFile", fileName, Convert.ToBase64String(memoryStream.ToArray()));
        }
    }

    // Helper method to determine whether to skip a column
    private bool ShouldExcludeColumn(string columnName, bool IsInventory)
    {
        // List of columns to be excluded
        var excludedColumns = new List<string>();
        if (IsInventory == false)
        {
            excludedColumns = new List<string>
            {
                "EntityId",
                "Entity",
                "TargetUser",
                "BuildingId",
                "PropertyId",
                "PropertyName",
                "EmployeeNumber",
                "Changes"
            };
        }
        else
        {
            excludedColumns = new List<string>
            {
                "EntityId",
                "TargetUser",
                "TargetUser",
                "BuildingId",
                "Entity",
                "PropertyId",
                "BuildingId",
                "PropertyName",
                "EmployeeNumber",
                "Changes"
            };
        }


        return excludedColumns.Contains(columnName);
    }

    // Helper method to set cell value based on data type
    private static void SetCellValue(IXLWorksheet worksheet, int row, int col, object value)
    {
        if (value is int intValue)
        {
            worksheet.Cell(row, col).Value = intValue;
        }
        else if (value is string stringValue)
        {
            worksheet.Cell(row, col).Value = stringValue;
        }
        else if (value is decimal decimalValue)
        {
            worksheet.Cell(row, col).Value = decimalValue;
        }
        else if (value is DateTime dateTimeValue)
        {
            worksheet.Cell(row, col).Value = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            worksheet.Cell(row, col).Value = string.Empty;
        }
    }
}