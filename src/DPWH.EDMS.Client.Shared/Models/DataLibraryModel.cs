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
    public Guid Id { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public bool IsDeleted { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
}

public class ConfigModel
{
    public string Value { get; set; }
    public string Id { get; set; }
    public string Section { get; set; }
    public string Office { get; set; }
    public string? DataType { get; set; }
}
public class SignatoryManagementModel
{
    public Guid Id { get; set; }
    public string DocumentType { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public string Office1 { get; set; }
    public string Office2 { get; set; }
    public int SignatoryNo { get; set; }
    public bool IsActive { get; set; }
    public string? DataType { get; set; } 
    public string EmployeeNumber { get; set; }
}