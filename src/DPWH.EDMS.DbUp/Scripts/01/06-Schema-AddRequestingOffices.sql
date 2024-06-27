--Create RequestingOffices table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'RequestingOffices' AND xtype = 'U')
BEGIN CREATE TABLE RequestingOffices (
	Id [nvarchar](50) PRIMARY KEY,	
	Name NVARCHAR(50) NOT NULL,
	ParentId [uniqueidentifier] NOT NULL,	
	NumberCode NVARCHAR(150) NOT NULL,
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(150),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(150) NULL,
);
END
GO

--  Create GeoLocation table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'[Geolocations]' AND xtype = 'U')
BEGIN CREATE TABLE [dbo].[Geolocations](
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [Type] [nvarchar](100) NOT NULL,
    [ParentId] [nvarchar](100) NOT NULL,
    [MyId] [nvarchar] (100) NOT NULL,
    [MyIdAdmin] [nvarchar] (100) NOT NULL,
    [ParentRef] [UNIQUEIDENTIFIER] NULL,
    [CreatedBy] [nvarchar](100) NOT NULL,
    [Created] [DATETIMEOFFSET](7) NOT NULL,
    [LastModifiedBy] [nvarchar](100) NULL,
    [LastModified] [DATETIMEOFFSET](7) NULL
    CONSTRAINT [PK_Geolocation] PRIMARY KEY CLUSTERED ( [Id] ASC )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]
END
GO

-- create parent (Id) -  child (w/ ParentKey) relation
ALTER TABLE [dbo].[Geolocations] ADD CONSTRAINT FK_ParentRef FOREIGN KEY (ParentRef) REFERENCES [dbo].[Geolocations](Id)
GO

-- Create Table Agencies
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'[Agencies]' AND xtype = 'U')
BEGIN CREATE TABLE [dbo].[Agencies](
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [AgencyNumberCode] [nvarchar](5) NULL,
    [AgencyCode] [nvarchar](10) NULL,
    [AgencyId] [nvarchar](20) NOT NULL,
    [AgencyName] [nvarchar](100) NOT NULL,
    [AttachedAgencyId] [nvarchar](20) NOT NULL,
    [AttachedAgencyName] [nvarchar](100) NOT NULL,
    [CreatedBy] [nvarchar](100) NOT NULL,
    [Created] [DATETIMEOFFSET](7) NOT NULL,
    [LastModifiedBy] [nvarchar](100) NULL,
    [LastModified] [DATETIMEOFFSET](7) NULL
    CONSTRAINT [PK_Agencies] PRIMARY KEY CLUSTERED ( [Id] ASC )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]
END
GO

-- Create Table DataSyncLogs Table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'[DataSyncLogs]' AND xtype = 'U')
BEGIN CREATE TABLE [dbo].[DataSyncLogs] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Type] [nvarchar](100) NOT NULL,
    [Result] [nvarchar](250) NULL,
    [Description] [nvarchar](250) NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
    [Created] [DATETIMEOFFSET](7) NOT NULL
    CONSTRAINT [PK_DataSyncLogs] PRIMARY KEY CLUSTERED ( [Id] ASC )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]
END
GO