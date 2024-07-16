IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'RecordRequests'
        AND COLUMN_NAME = 'OtherPurpose'
) BEGIN
    ALTER TABLE
        RecordRequests
    ADD
        OtherPurpose NVARCHAR(200) NULL;
END