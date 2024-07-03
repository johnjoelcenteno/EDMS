--Create 
IF OBJECT_ID('dbo.ControlNumberSequence', 'SO') IS NULL 
BEGIN
    --DROP SEQUENCE dbo.ControlNumberSequence
    CREATE SEQUENCE [dbo].[ControlNumberSequence] 
     AS [int]
     START WITH 1
     INCREMENT BY 1
     MINVALUE 1
     MAXVALUE 999999
     CACHE
END
GO

--Create RecordRequests table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'RecordRequests' AND xtype = 'U')
BEGIN CREATE TABLE RecordRequests (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	ControlNumber [int] NOT NULL,	
	EmployeeNumber NVARCHAR(50) NOT NULL,	
	ClaimantType NVARCHAR(50),
	DateRequested [DATETIMEOFFSET](7) NOT NULL,
	RepresentativeName NVARCHAR(150) NULL,
	ValidId [uniqueidentifier] NULL,
	AuthorizationDocumentId [uniqueidentifier] NULL,	
	Purpose NVARCHAR(150),
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
	[Type] NVARCHAR(100) NOT NULL,
	[DocumentTypeId] [uniqueidentifier] NOT NULL,
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

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RecordTypes')
BEGIN CREATE TABLE RecordTypes (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Section NVARCHAR(150) NULL,
    Office NVARCHAR(150) NULL,
    IsActive BIT NOT NULL DEFAULT 0,
    CreatedBy NVARCHAR(150) NOT NULL,
    Created DateTimeOffset NOT NULL,
    LastModifiedBy NVARCHAR(150) NULL,
    LastModified DateTimeOffset NULL
);
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Signatories')
BEGIN CREATE TABLE Signatories (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    DocumentType NVARCHAR(150) NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Position NVARCHAR(150) NOT NULL,
    Office1 NVARCHAR(150) NOT NULL,
    Office2 NVARCHAR(150) NULL,
    SignatoryNo INT DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedBy NVARCHAR(150) NOT NULL,
    Created DateTimeOffset NOT NULL,
    LastModifiedBy NVARCHAR(150) NULL,
    LastModified DateTimeOffset NULL
);

END

--Constraints
ALTER TABLE [dbo].[RequestedRecords]  WITH NOCHECK ADD CONSTRAINT [FK_RequestedRecords_RecordRequest_Id] FOREIGN KEY([RecordRequestId])
REFERENCES [dbo].[RecordRequests] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequestedRecords] CHECK CONSTRAINT [FK_RequestedRecords_RecordRequest_Id]
GO

ALTER TABLE [dbo].[RequestedRecords]  WITH NOCHECK ADD CONSTRAINT [FK_RequestedRecords_RecordTypes_Id] FOREIGN KEY([RecordTypeId])
REFERENCES [dbo].[RecordTypes] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequestedRecords] CHECK CONSTRAINT [FK_RequestedRecords_RecordTypes_Id]
GO

ALTER TABLE [dbo].[RecordRequestDocuments]  WITH NOCHECK ADD CONSTRAINT [FK_RecordRequestDocuments_RecordRequests_Id] FOREIGN KEY([RecordRequestId])
REFERENCES [dbo].[RecordRequests] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RecordRequestDocuments] CHECK CONSTRAINT [FK_RecordRequestDocuments_RecordRequests_Id]
GO