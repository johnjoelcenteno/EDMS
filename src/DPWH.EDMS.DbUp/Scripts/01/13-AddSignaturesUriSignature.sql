IF NOT EXISTS (
    SELECT
        *
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'Signatories'
        AND COLUMN_NAME = 'UriSignature'
) BEGIN
    ALTER TABLE
        Signatories
    ADD
        [UriSignature] NVARCHAR(MAX) NULL
END