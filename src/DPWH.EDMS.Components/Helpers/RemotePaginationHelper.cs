namespace DPWH.EDMS.Components.Helpers
{
    public static class RemotePaginationHelper
    {
        public static string ConvertTelerikFilterOperatorToKendo(Telerik.DataSource.FilterOperator tOperator)
        {
            switch (tOperator)
            {
                case Telerik.DataSource.FilterOperator.Contains:
                    return "contains";
                case Telerik.DataSource.FilterOperator.IsEqualTo:
                    return "eq";
                case Telerik.DataSource.FilterOperator.IsNotEqualTo:
                    return "neq";
                case Telerik.DataSource.FilterOperator.StartsWith:
                    return "startswith";
                case Telerik.DataSource.FilterOperator.DoesNotContain:
                    return "doesnotcontain";
                case Telerik.DataSource.FilterOperator.EndsWith:
                    return "endswith";
                case Telerik.DataSource.FilterOperator.IsNull:
                    return "isnull";
                case Telerik.DataSource.FilterOperator.IsNotNull:
                    return "isnotnull";
                default:
                    return tOperator.ToString().ToLower() ?? string.Empty;
            }
        }

        //public static DataSourceRequest FetchPagedData(GridReadEventArgs args)
        //{
        //    var filters = new List<Filter>();
        //    var sorts = new List<Sort>();
        //    const string DEFAULT_F_LOGIC = "and";

        //    foreach (Telerik.DataSource.FilterDescriptor item in args.Request.Filters)
        //    {
        //        filters.Add(
        //            new Filter()
        //            {
        //                Field = item.Member,
        //                Operator = ConvertTelerikFilterOperatorToKendo(item.Operator),
        //                Value = item.Value,
        //                Logic = null,
        //            });
        //    }

        //    foreach (Telerik.DataSource.SortDescriptor item in args.Request.Sorts)
        //    {
        //        sorts.Add(
        //            new Sort()
        //            {
        //                Dir = item.SortDirection.ToString(),
        //                Field = item.Member
        //            });
        //    }

        //    var dsFilters = new Filter()
        //    {
        //        Field = null,
        //        Operator = null,
        //        Value = null,
        //        Logic = DEFAULT_F_LOGIC,
        //        Filters = filters
        //    };

        //    var dsReq = new DataSourceRequest()
        //    {
        //        Take = args.Request.PageSize,
        //        Skip = (args.Request.Page - 1) * args.Request.PageSize,
        //        Filter = (filters.Count > 0 && filters != null) ? dsFilters : null,
        //        Sort = (sorts.Count > 0) ? sorts : new List<Sort>(),
        //    };

        //    return dsReq;
        //}
    }
}
