-- Create MenuItems table if it does not exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = N'MenuItems' AND xtype = 'U')
BEGIN
    CREATE TABLE MenuItems (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Text NVARCHAR(200) NULL,
        Url NVARCHAR(350) NULL,
        Icon NVARCHAR(200) NULL,
        Expanded BIT NOT NULL,
        Level INT NOT NULL,
        SortOrder INT NOT NULL,
        AuthorizedRoles NVARCHAR(MAX) NULL, -- Storing list of roles as a delimited string
        ParentId UNIQUEIDENTIFIER NULL,
        Created DATETIMEOFFSET(7) NOT NULL,
        CreatedBy NVARCHAR(150) NOT NULL,
        LastModified DATETIMEOFFSET(7) NULL,
        LastModifiedBy NVARCHAR(150) NULL
    );
END

-- Add foreign key constraint with NO ACTION to prevent cascading issues
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = N'FK_MenuItems_ParentId')
BEGIN
    ALTER TABLE [dbo].[MenuItems] ADD CONSTRAINT [FK_MenuItems_ParentId] FOREIGN KEY([ParentId])
    REFERENCES [dbo].[MenuItems] ([Id])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION;
END
