--Create UserRecords table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'Records' AND xtype = 'U')
BEGIN CREATE TABLE Records (
	Id UNIQUEIDENTIFIER PRIMARY KEY,	
	EmployeeId NVARCHAR(50) NOT NULL,
	RecordTypeId [uniqueidentifier] NOT NULL,	
	RecordName NVARCHAR(150) NOT NULL,
	RecordUri NVARCHAR(250) NULL,	
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(150),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(150) NULL,
);
END
GO