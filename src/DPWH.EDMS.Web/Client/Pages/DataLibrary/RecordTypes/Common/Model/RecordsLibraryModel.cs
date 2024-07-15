namespace DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;

public class RecordsLibraryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Section { get; set; }
    public string Category { get; set; } = "System";
    public string Office { get; set; }
    public bool IsActive { get; set; }
}

