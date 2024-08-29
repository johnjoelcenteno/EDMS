IF NOT EXISTS (
    SELECT
        1
    FROM
        INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'Signatories'
        AND COLUMN_NAME = 'EmployeeNumber'
) BEGIN
ALTER TABLE
    Signatories
ADD
    EmployeeNumber NVARCHAR(150) NULL
END