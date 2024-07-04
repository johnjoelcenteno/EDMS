IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'RecordRequests'
        AND COLUMN_NAME = 'FullName'
) BEGIN
ALTER TABLE
    RecordRequests
ADD
    FullName NVARCHAR(150);

END