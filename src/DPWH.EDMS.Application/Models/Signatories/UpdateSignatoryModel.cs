﻿namespace DPWH.EDMS.Application;

public class UpdateSignatoryModel
{
    public string DocumentType { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public string Office1 { get; set; }
    public string? Office2 { get; set; }
    public int SignatoryNo { get; set; }
    public bool IsActive { get; set; }
    public string? EmployeeNumber { get; set; }
}
