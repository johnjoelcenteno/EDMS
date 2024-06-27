using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.DataSource;

namespace DPWH.EDMS.Client.Shared.MockModels;
public class RecordModel
{
    public Guid Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Office { get; set; }
    public string BureauServiceDivisionSectionUnit { get; set; }
}
