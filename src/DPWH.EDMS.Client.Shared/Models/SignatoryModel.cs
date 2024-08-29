using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.Models;

public class SignatoryModel
{
    public string Id { get; set; }
    public string DocumentType { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public string Office1 { get; set; }
    public string Office2 { get; set; }
    public int SignatoryNo { get; set; }
    public bool IsActive { get; set; }
    public string EmployeeNumber { get; set; }

}