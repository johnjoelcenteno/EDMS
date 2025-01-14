﻿namespace DPWH.EDMS.Client.Shared.MockModels;

public class EmployeeModel
{
    public Guid Id { get; set; }
    public string ControlNumber { get; set; }
    public DateTime DateRequested { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleInitial { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string RecordRequested { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
    public string UserAccess { get; set; }
}
