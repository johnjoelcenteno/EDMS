-- Check if the DocumentType column already exists
IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'RequestedRecords'
        AND COLUMN_NAME = 'DocumentType'
) BEGIN
    ALTER TABLE
        RequestedRecords
    ADD
        [DocumentType] NVARCHAR(50) NOT NULL;
END