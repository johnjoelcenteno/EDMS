IF NOT EXISTS (
    SELECT 1 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'ChangeLogs' 
    AND COLUMN_NAME = 'ControlNumber'
)
ALTER TABLE [dbo].[ChangeLogs]
ADD ControlNumber NVARCHAR(50) NULL;
GO