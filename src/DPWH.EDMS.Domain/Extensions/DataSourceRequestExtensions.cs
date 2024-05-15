using System.Text.Json;
using KendoNET.DynamicLinq;

namespace DPWH.EDMS.Domain.Extensions;

public static class DataSourceRequestExtensions
{
    public static DataSourceRequest FixSerialization(this DataSourceRequest request)
    {
        ProcessFilter(request.Filter);
        return request;
    }

    private static void ProcessFilter(Filter? filter)
    {
        if (filter is null)
        {
            return;
        }

        if (filter.Value?.GetType() == typeof(JsonElement))
        {
            var json = (JsonElement)filter.Value;
            filter.Value = json.ValueKind switch
            {
                JsonValueKind.Null => null,
                JsonValueKind.Number => json.GetDecimal(),
                JsonValueKind.True or JsonValueKind.False => json.GetBoolean(),
                _ => json.GetString()
            };
        }

        // Field to Pascal Case
        if (filter.Field != null)
        {
            filter.Field = filter.Field.Substring(0, 1).ToUpper() + filter.Field.Substring(1);
        }

        if (filter.Filters is null)
        {
            return;
        }

        // Recurse
        foreach (var f in filter.Filters)
        {
            ProcessFilter(f);
        }
    }
}