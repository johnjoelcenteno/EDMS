-- Remove columns from signatories table
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Signatories'
    AND COLUMN_NAME = 'UriSignature'
)
BEGIN
    -- Drop the 'UriSignature' column
    ALTER TABLE Signatories
    DROP COLUMN [UriSignature];
END
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Signatories'
    AND COLUMN_NAME = 'EmployeeNumber'
)
BEGIN
    -- Drop the 'EmployeeNumber' column
    ALTER TABLE Signatories
    DROP COLUMN [EmployeeNumber];
END


IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Signatories'
    AND COLUMN_NAME = 'EmployeeNumber'
)
BEGIN
    -- Drop the 'EmployeeNumber' column
    ALTER TABLE Signatories
    DROP COLUMN [EmployeeNumber];
END


-- Create UserProfileDocuments Table
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserProfileDocuments')
BEGIN 
    CREATE TABLE UserProfileDocuments (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,   
        EmployeeNumber VARCHAR(50) NOT NULL,           
        UriSignature NVARCHAR(MAX) NOT NULL,        
        Created DATETIMEOFFSET(7) NOT NULL,
        CreatedBy NVARCHAR(150) NOT NULL,
        LastModified DATETIMEOFFSET(7) NULL,
        LastModifiedBy NVARCHAR(150) NULL
    );
END;