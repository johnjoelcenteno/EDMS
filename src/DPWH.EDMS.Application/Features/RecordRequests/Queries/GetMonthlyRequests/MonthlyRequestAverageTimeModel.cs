using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetMonthlyRequests
{
    public class MonthlyRequestAverageTimeModel
    {
        public double? HRMDMonthlyAverage { get; set; }
        public double? RMDMonthlyAverage { get; set; }
        public double? Both { get; set; }
    }
}
