--Create 
IF OBJECT_ID('dbo.ControlNumberSequence', 'SO') IS NULL 
BEGIN
    --DROP SEQUENCE dbo.ControlNumberSequence
    CREATE SEQUENCE [dbo].[ControlNumberSequence] 
     AS [int]
     START WITH 1
     INCREMENT BY 1
     MINVALUE 1
     MAXVALUE 999999
     CACHE
END
GO