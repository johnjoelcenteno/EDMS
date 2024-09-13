IF EXISTS (
	 SELECT
    [so].[name] AS [trigger_name]
	FROM sysobjects AS [so]
	INNER JOIN sysobjects AS so2 ON so.parent_obj = so2.Id
	WHERE [so].[type] = 'TR'
) BEGIN 
	DECLARE @TriggerName NVARCHAR(100);


	--DROP TRIGGER UpdateDateReleased
	SELECT
	@TriggerName = [so].name
	FROM sysobjects AS [so]
	INNER JOIN sysobjects AS so2 ON so.parent_obj = so2.Id
	WHERE [so].[type] = 'TR' AND [so].[name] = 'UpdateDateReleased'

	IF @TriggerName IS NOT NULL BEGIN
		EXEC('DROP TRIGGER ' + @TriggerName)
	END

END;