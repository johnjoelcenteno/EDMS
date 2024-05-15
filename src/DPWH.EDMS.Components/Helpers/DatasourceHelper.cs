using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Components.Helpers
{
    public static class DataSourceHelper
    {
        // OPERATORS
        public static readonly string CONTAINS_OPERATOR = "contains";
        public static readonly string EQUAL_OPERATOR = "eq";
        public static readonly string NOT_EQUAL_OPERATOR = "neq";
        public static readonly string GREATER_THAN_OPERATOR = "gt";
        public static readonly string GREATER_THAN_OR_EQUAL_OPERATOR = "gte";
        public static readonly string LESS_THAN_OPERATOR = "lt";
        public static readonly string LESS_THAN_OR_EQUAL_OPERATOR = "lte";
        public static readonly string STARTS_WITH_OPERATOR = "startswith";
        public static readonly string ENDS_WITH_OPERATOR = "endswith";
        public static readonly string IS_NULL_OPERATOR = "isnull";
        public static readonly string IS_NOT_NULL_OPERATOR = "isnotnull";
        public static readonly string IS_EMPTY_OPERATOR = "isempty";
        public static readonly string NEWEST = "newest";
        public static readonly string OLDEST = "oldest";

        // LOGICS
        public static readonly string IS_NOT_EMPTY_OPERATOR = "isnotempty";
        public static readonly string AND_LOGIC = "and";
        public static readonly string OR_LOGIC = "or";
        public static readonly string NOT_LOGIC = "not";

        // SORTING DIRECTIONS
        public static readonly string ASC_DIR = "asc";
        public static readonly string DESC_DIR = "desc";
    }
}
