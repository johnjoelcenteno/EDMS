using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetMonthlyRequests
{
    public class MonthlyRequestAverageTimeModel
    {
        public decimal? HRMDMonthlyAverage { get; set; }
        public decimal? RMDMonthlyAverage { get; set; }
        public decimal? Both { get; set; }
    }
}
