-- Check if the RecordRequests column already exists
IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'RecordRequests'
        AND COLUMN_NAME = 'HRMDRequestStatus'
) BEGIN
    ALTER TABLE
        RecordRequests
    ADD
        [HRMDRequestStatus] NVARCHAR(50) NULL,
        [RMDRequestStatus] NVARCHAR(50) NULL;
END