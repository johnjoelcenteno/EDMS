using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.MockModels;

public class DocumentRequestModel
{
    public string ControlNumber { get; set; }
    public List<string> RecordsRequested { get; set; }
    public DateTime DateRequested { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
}
