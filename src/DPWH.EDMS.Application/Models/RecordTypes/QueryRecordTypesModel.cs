﻿namespace DPWH.EDMS.Application;

public class QueryRecordTypesModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Section { get; set; }
    public string Office { get; set; }
    public bool IsActive { get; set; }
}