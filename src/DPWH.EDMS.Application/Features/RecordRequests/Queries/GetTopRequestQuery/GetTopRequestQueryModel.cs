using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetTopRequestQuery;

public class GetTopRequestQueryModel
{
    public string RecordName { get; set; }
    public long Total {  get; set; }
}
