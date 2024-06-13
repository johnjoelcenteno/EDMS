using System.ComponentModel.DataAnnotations;
using Telerik.FontIcons;

namespace DPWH.EDMS.Client.Shared.Models;

public class DataLibraryModel
{
    public string Name { get; set; }
    public string Url { get; set; }
    public FontIcon Icon { get; set; }
}

public class DataManagementModel
{
    public string Value { get; set; }
    public string CreatedBy { get; set; }
}

public class ConfigModel
{
    [Display(Name = "Value*:")]
    [Required(ErrorMessage = "You must specify Value.")]
    public string Value { get; set; }
}