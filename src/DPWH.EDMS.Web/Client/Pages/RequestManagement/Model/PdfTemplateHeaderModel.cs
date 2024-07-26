namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.Model;

public class PdfTemplateHeaderModel
{
    public string Logo { get; set; }
    public string Title { get; set; }
    public string DocumentSubtitle { get; set; }
    public string Subtitle { get; set; }
    public string DocumentTitle { get; set; }
    public DateTime IssueDate { get; set; }
    public string DocumentCode { get; set; }
    public string PageTitle { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPage { get; set; }
}