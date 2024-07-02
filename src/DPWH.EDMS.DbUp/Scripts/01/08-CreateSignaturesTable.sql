IF NOT EXISTS (
    SELECT
        1
    FROM
        INFORMATION_SCHEMA.TABLES
    WHERE
        TABLE_NAME = 'Signatories'
) BEGIN CREATE TABLE Signatories (
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