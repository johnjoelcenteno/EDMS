IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'RequestedRecords'
        AND COLUMN_NAME = 'Office'
) BEGIN
ALTER TABLE
    RequestedRecords
ADD
    Office NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [IsAvailable] [bit] NOT NULL DEFAULT 0,
    [Uri] NVARCHAR(250) NULL;
END