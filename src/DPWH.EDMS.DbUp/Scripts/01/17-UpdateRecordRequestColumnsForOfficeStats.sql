IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'RecordRequests'
        AND COLUMN_NAME = 'HRMDNoDaysUntilReleased'
)
BEGIN
    ALTER TABLE RecordRequests
    ADD HRMDNoDaysUntilReleased DECIMAL(10, 2) NULL,
        RMDNoDaysUntilReleased DECIMAL(10, 2) NULL;
END
GO

CREATE TRIGGER UpdateDateReleased
ON RecordRequests
AFTER UPDATE
AS
BEGIN
    DECLARE @ControlNumber INT;
    DECLARE @NoDays DECIMAL(10, 2);
    DECLARE @IsHRMDStatusReleased BIT;
    DECLARE @IsRMDStatusReleased BIT;


    SELECT @ControlNumber = ControlNumber,
           @NoDays = DATEDIFF(DAY, Created, GETDATE()) / 1.0,
           @IsHRMDStatusReleased =  CASE 
                                WHEN INSERTED.HRMDRequestStatus = 'Released' 
                                THEN 1 
                                ELSE 0 
                             END,
           @IsRMDStatusReleased =  CASE 
                                WHEN INSERTED.RMDRequestStatus = 'Released' 
                                THEN 1 
                                ELSE 0 
                             END
    FROM INSERTED;

    IF UPDATE(HRMDRequestStatus)
    BEGIN
        UPDATE RecordRequests
        SET HRMDNoDaysUntilReleased = CASE WHEN @IsHRMDStatusReleased = 1 THEN @NoDays ELSE HRMDNoDaysUntilReleased END
        WHERE ControlNumber = @ControlNumber;
    END

    IF UPDATE(RMDRequestStatus)
    BEGIN
        UPDATE RecordRequests
        SET RMDNoDaysUntilReleased = CASE WHEN @IsRMDStatusReleased = 1 THEN @NoDays ELSE RMDNoDaysUntilReleased END
        WHERE ControlNumber = @ControlNumber;
    END
END;
