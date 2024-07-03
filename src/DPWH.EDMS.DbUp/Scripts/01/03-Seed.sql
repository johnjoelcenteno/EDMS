
-- ConfigSettings
INSERT INTO [dbo].[ConfigSettings] (Id, [Name], [Value], [Description], CreatedBy, Created) VALUES (NEWID(), 'LicenseMaxLimit', 600, 'License limit', 'system', GETDATE());

--Insert Data libraries

--Valid IDs
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Passport', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'SSS', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'GSIS', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Driver License', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'PRC', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'OWWA', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'iDOLE', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Voter''s ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Voter''s Certification', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Firearms License', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Senior Citizen', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'PWD', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'NBI', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'PhilHealth', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'GOCC', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'IBP', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'School ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'ePassport', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'TIN ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Postal ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Barangay Certification', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'GSIS e-CARD', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Seaman''s Book', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'NCWDP ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'DSWD ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Company ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Police Clearance', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Barangay Clearance', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Cedula', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Government Service Record', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Elementary or High School Form 137', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Transcript of Records', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'Land Title', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'PSA Marriage Contract', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'ValidIDs', 'PSA Birth Certificate', 0, 'system', GETDATE());

--AuthorizationDocuments
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'AuthorizationDocuments', 'Authorization Letter', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'AuthorizationDocuments', 'Special Power of Attorney', 0, 'system', GETDATE());

--Purposes
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'Purposes', 'Scholarship / Training Application', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'Purposes', 'Loan', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'Purposes', 'Visa', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'Purposes', 'Promotion', 0, 'system', GETDATE());

-- RMD
INSERT INTO [RecordTypes] VALUES 
    (NEWID(),N'Department Order',N'DPWH Issuances',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Special Order',N'DPWH Issuances',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Department Memorandum Circular',N'DPWH Issuances',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Unnumbered Memorandum',N'DPWH Issuances',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Contract of Service',N'Employee Records',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Notice of Salary Adjustment (NOSA)',N'Employee Records',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Notice of Salary Increment (NOSI)',N'Employee Records',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Statements of Assets and Liabilities and Net Worth (SALN)',N'Employee Records',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Travel Order',N'Employee Records',N'Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Approved Appointments',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Position Description Form (PDF)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Service Record (Separated)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Employee Leave Card',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Leave Application (Terminal Leave only)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Notice of Salary Adjustment (NOSA)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Personal Data Sheet (PDS) / Information Sheet (Must be latest)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N' Administrative Case / Civil case  Decisions',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Training Certificates . Ratings',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Travel Directive, Certificate of Appearance',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Authority to Travel for Personal Reason',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Memorandum Receipt for Equipment (MT)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'GSIS Forms (Retirement Information for Membership)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Memo (Designation, Directive)',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Daily Wage Appointment / Plantilla',N'Employee Records',N'Non-Current Section',N'RMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL);

-- HRMD
INSERT INTO [RecordTypes] VALUES 
    (NEWID(),N'Service Record (Active)',N'Employee Records',N'Record Management Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employment',N'Employee Records',N'Record Management Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employment and Compensation',N'Employee Records',N'Record Management Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Last Day in Service',N'Employee Records',N'Record Management Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employer-Employee Relationship',N'Employee Records',N'Record Management Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employer-Employee Relationship',N'Employee Records',N'Record Management Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certification of No Loan Deductions',N'Employee Records',N'Record Management Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Salary Index',N'Employee Records',N'Integrated Payroll and Personnel Information',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Service Record (Active)',N'Employee Records',N'Employee Welfare and Benefits Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employment',N'Employee Records',N'Employee Welfare and Benefits Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employment and Compensation',N'Employee Records',N'Employee Welfare and Benefits Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Last Day in Service',N'Employee Records',N'Employee Welfare and Benefits Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employer-Employee Relationship',N'Employee Records',N'Employee Welfare and Benefits Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certificate of Employer-Employee Relationship',N'Employee Records',N'Employee Welfare and Benefits Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL),
	(NEWID(),N'Certification of No Loan Deductions',N'Employee Records',N'Employee Welfare and Benefits Section',N'HRMD',1,N'System','2024-07-02 01:14:45.360000',NULL,NULL);

-- Archived
INSERT INTO [RecordTypes] VALUES 
	(NEWID(), N'Approved Appointments', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Position Description Form (PDF)', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Service Records', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Employee Leave Card', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Leave Application (Terminal Leave only)', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Notice of Salary Adjustment (NOSA)', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Personal Data Sheet (PDS) / Information Sheet (must be latest)', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Administrative Case / Civil Case/Decisions', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Training Certificates / Ratings', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Travel Directive, Certificate of Appearance', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Authority to Travel for Personal Reason', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Memorandum Receipt for Equipment (MR)', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'GSIS Forms (Retirement, Information for Membership)', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Memo (Designation, Directive)', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL),
	(NEWID(), N'Daily Wage Appointment / Plantilla', N'Archived', N'Archived', NULL, 1, N'System', '2024-07-02 01:14:45.360000', NULL, NULL);


