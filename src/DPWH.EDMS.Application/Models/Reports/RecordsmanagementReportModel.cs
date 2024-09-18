using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Models.Reports
{
    public class RecordsmanagementReportModel
    {
        public int Id { get; set; }
        public DateTimeOffset? ReleaseDate { get; set; }
        public string Status { get; set; }
    }
}
