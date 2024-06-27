﻿using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application;

public record class CreateRecordTypeModel(
    string Name,
    string Category,
    string Section,
    string Office,
    bool IsActive
);