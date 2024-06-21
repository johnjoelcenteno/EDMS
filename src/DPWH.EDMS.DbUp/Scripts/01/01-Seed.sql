
-- ConfigSettings
INSERT INTO [dbo].[ConfigSettings] (Id, [Name], [Value], [Description], CreatedBy, Created) VALUES (NEWID(), 'LicenseMaxLimit', 600, 'License limit', 'system', GETDATE());

--Insert Data libraries

INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'RecordTypes', 'PDS', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'RecordTypes', 'Service Record', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'RecordTypes', 'Leave Card', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'RecordTypes', 'Salary Adjustment', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'RecordTypes', 'Training Certificates', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'RecordTypes', 'Memorandum', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'RecordTypes', 'Appointment Papers', 0, 'system', GETDATE());

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

--Secondary IDs
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'TIN ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Postal ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Barangay Certification', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'GSIS e-CARD', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Seaman''s Book', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'NCWDP ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'DSWD ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Company ID', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Police Clearance', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Barangay Clearance', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Cedula', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Government Service Record', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Elementary or High School Form 137', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Transcript of Records', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'Land Title', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'PSA Marriage Contract', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'SecondaryIDs', 'PSA Birth Certificate', 0, 'system', GETDATE());


--Secondary IDs
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'AuthorizationDocuments', 'Authorization Letter', 0, 'system', GETDATE());
INSERT INTO [dbo].[DataLibraries] (Id, [Type], [Value], IsDeleted, CreatedBy, Created) VALUES (NEWID(), 'AuthorizationDocuments', 'Special Power of Attorney', 0, 'system', GETDATE());

