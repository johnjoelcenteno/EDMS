IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = 'EmployeeRecords' AND xtype = 'U')
BEGIN CREATE TABLE EmployeeRecords (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50),
	LastName NVARCHAR(50) NOT NULL,
	Office NVARCHAR(100),
	Email NVARCHAR(100),
	MobileNumber NVARCHAR(15),
	EmployeeNumber NVARCHAR(50) NOT NULL UNIQUE,
	EmployeeId NVARCHAR(75) NOT NULL UNIQUE,
	RegionCentralOffice NVARCHAR(100),
	DistrictBureauService NVARCHAR(100),
	Position NVARCHAR(100),
	Designation NVARCHAR(100),
	Role NVARCHAR(150),
	UserAccess NVARCHAR(50),
	Department NVARCHAR(150),
	RegionalOfficeRegion NVARCHAR(150),
	RegionalOfficeProvince NVARCHAR(150),
	DistrictEngineeringOffice NVARCHAR(150),
	DesignationTitle NVARCHAR(150),
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(50),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(50) NULL
);
END 

--Create RecordRequests table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'RecordRequests' AND xtype = 'U')
BEGIN CREATE TABLE RecordRequests (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	ControlNumber NVARCHAR(50) NOT NULL,
	EmployeeNumber NVARCHAR(50) NOT NULL,
	IsActiveEmployee [bit] NOT NULL,
	ClaimantType NVARCHAR(50),
	DateRequested [DATETIMEOFFSET](7) NOT NULL,
	RepresentativeName NVARCHAR(150) NULL,
	ValidId NVARCHAR(150) NULL,
	ValidIdUri NVARCHAR(250) NULL,
	SupportingDocument NVARCHAR(150) NULL,	
	SupportingDocumentUri NVARCHAR(250) NULL,	
	Purpose NVARCHAR(100),
	Status NVARCHAR(50),
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(150),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(150) NULL,
);
END

--Create RecordRequestDocuments table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'RecordRequestDocuments' AND xtype = 'U')
BEGIN CREATE TABLE RecordRequestDocuments (
	Id UNIQUEIDENTIFIER NOT NULL,
	RecordRequestId [uniqueidentifier] NULL,
	[Name] NVARCHAR(250) NOT NULL,
	[Filename] NVARCHAR(250) NOT NULL,	
	[Type] NVARCHAR(100),	
	[FileSize] [bigint] NULL,
	Uri NVARCHAR(350) NULL,	
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(150) NOT NULL,
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(150) NULL
	CONSTRAINT [PK_RecordRequestDocuments] PRIMARY KEY CLUSTERED 
	([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

--Create RequestedRecords table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'RequestedRecords' AND xtype = 'U')
BEGIN CREATE TABLE RequestedRecords (
	Id UNIQUEIDENTIFIER NOT NULL,
	RecordRequestId [uniqueidentifier] NOT NULL,
	RecordTypeId [uniqueidentifier] NOT NULL,	
	RecordType NVARCHAR(250) NOT NULL	
	CONSTRAINT [PK_RequestedRecords] PRIMARY KEY CLUSTERED 
	([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

--Constraints
ALTER TABLE [dbo].[RequestedRecords]  WITH NOCHECK ADD CONSTRAINT [FK_RequestedRecords_RecordRequest_Id] FOREIGN KEY([RecordRequestId])
REFERENCES [dbo].[RecordRequests] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequestedRecords] CHECK CONSTRAINT [FK_RequestedRecords_RecordRequest_Id]
GO

ALTER TABLE [dbo].[RequestedRecords]  WITH NOCHECK ADD CONSTRAINT [FK_RequestedRecords_DataLibraries_Id] FOREIGN KEY([RecordTypeId])
REFERENCES [dbo].[DataLibraries] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequestedRecords] CHECK CONSTRAINT [FK_RequestedRecords_DataLibraries_Id]
GO