-- Create Table ConfigSettings
CREATE TABLE [dbo].[ConfigSettings](
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [Value] [nvarchar](250) NOT NULL,
    [Description] [nvarchar](250) NULL,    
    [CreatedBy] [nvarchar](150) NOT NULL,
    [Created] [DATETIMEOFFSET](7) NOT NULL,
    [LastModifiedBy] [nvarchar](150) NULL,
    [LastModified] [DATETIMEOFFSET](7) NULL
) ON [PRIMARY];
GO

-- Create Table SystemLogs
CREATE TABLE [dbo].[SystemLogs](
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [Version] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](150) NULL,
    [CreatedBy] [nvarchar](150) NOT NULL,
    [Created] [DATETIMEOFFSET](7) NOT NULL
    CONSTRAINT [PK_SystemLogs] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    ) WITH 
    (
        PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO

--Create table AuditLogs
CREATE TABLE [dbo].[AuditLogs](
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [EntityId] [nvarchar](50) NOT NULL,
    [Entity] [nvarchar](50) NOT NULL,
    [PropertyId] [nvarchar](100) NULL,
    [PropertyName] [nvarchar](400) NULL,
    [UserName] [nvarchar](50) NOT NULL,
    [FirstName] [nvarchar](50) NULL,
    [LastName] [nvarchar](50) NULL,
    [EmployeeNumber] [nvarchar](100) NULL,
    [Data] [nvarchar](max) NOT NULL,
    [Action] [nvarchar](20) NOT NULL,
    [CreatedBy] [nvarchar](150) NOT NULL,
    [Created] [DATETIMEOFFSET](7) NOT NULL
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    ) WITH
    (
      PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO

-- Create Table Data Library
CREATE TABLE [dbo].[DataLibraries](
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [Type] [nvarchar](50) NOT NULL,
    [Value] [nvarchar](100) NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT 0,
    [CreatedBy] [nvarchar](150) NOT NULL,
    [Created] [DATETIMEOFFSET](7) NOT NULL,
    [LastModifiedBy] [nvarchar](150) NULL,
    [LastModified] [DATETIMEOFFSET](7) NULL
    CONSTRAINT [PK_DataLibraries] PRIMARY KEY CLUSTERED ( [Id] ASC )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]
GO