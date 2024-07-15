IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'RecordRequests'
        AND COLUMN_NAME = 'Email'
) BEGIN
    ALTER TABLE
        RecordRequests
    ADD
        Email NVARCHAR(150) NULL;
END

IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'RecordRequests'
        AND COLUMN_NAME = 'Remarks'
) BEGIN
    ALTER TABLE
        RecordRequests
    ADD
        Remarks NVARCHAR(200) NULL;
END

-- Make EmployeeNumber nullable
ALTER TABLE RecordRequests
ALTER COLUMN EmployeeNumber NVARCHAR(50) NULL;