using System.ComponentModel.DataAnnotations;
using Telerik.FontIcons;


namespace DPWH.EDMS.Client.Shared.Models
{
    public class LookupRecordModels
    {
        public Guid Id { get; set; }
        public string RecordName { get; set; }
        public string EmployeeId { get; set; }
        public Guid RecordTypeId { get; set; }
        public string RecordUri { get; set; }
        public string DocVersion { get; set; }
        public List<RecordModels> Documents { get; set; }

    }
    public class RecordModels{
    public Guid Id { get; set; }
    public Guid RecordTypeId { get; set; }
    public string EmployeeId { get; set; }
    public string RecordName { get; set; }
    public string RecordUri { get; set; }
    public string DocVersion { get; set; }

    }
}
