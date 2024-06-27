IF NOT EXISTS (
    SELECT
        1
    FROM
        INFORMATION_SCHEMA.TABLES
    WHERE
        TABLE_NAME = 'RecordTypes'
) BEGIN CREATE TABLE RecordTypes (
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