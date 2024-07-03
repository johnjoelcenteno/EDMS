-- Create Table ChangeLogs Table
CREATE TABLE [dbo].[ChangeLogs] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [EntityId] [nvarchar](40) NOT NULL,
    [Entity] [nvarchar](50) NOT NULL,
    [PropertyId] [nvarchar](50) NULL,
    [BuildingId] [nvarchar](50) NULL,
    [PropertyName] [nvarchar](500) NULL,
    [ActionType] [nvarchar](40) NOT NULL,
    [UserId] [nvarchar](40) NULL,
    [UserName] [nvarchar](50) NULL,
    [FirstName] [nvarchar](50) NULL,
    [LastName] [nvarchar](50) NULL,
    [MiddleInitial] [nvarchar](50) NULL,
    [EmployeeNumber] [nvarchar](50) NULL,
    [ActionDate] [DATETIMEOFFSET](7) NOT NULL
    CONSTRAINT [PK_ChangeLogs] PRIMARY KEY CLUSTERED ( [Id] ASC )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]
    GO

CREATE INDEX IX_ChangeLogs_Entity_ActionDate ON dbo.ChangeLogs (Entity, ActionDate);
GO

-- Create Table ChangeLogItems Table
CREATE TABLE [dbo].[ChangeLogItems] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Field] [nvarchar](50) NOT NULL,
    [From] [nvarchar](1000) NULL,
    [To] [nvarchar](1000) NULL,
    [ChangeLogId] [int] NOT NULL,
    CONSTRAINT [PK_ChangeLogItems] PRIMARY KEY CLUSTERED ( [Id] ASC )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]
    GO

ALTER TABLE [dbo].[ChangeLogItems]  WITH NOCHECK ADD CONSTRAINT [FK_ChangeLogItems_ChangeLogs] FOREIGN KEY ([ChangeLogId]) REFERENCES [dbo].[ChangeLogs] ([Id])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ChangeLogItems] CHECK CONSTRAINT [FK_ChangeLogItems_ChangeLogs]
    GO