IF EXISTS (
	 SELECT
    [so].[name] AS [trigger_name]
	FROM sysobjects AS [so]
	INNER JOIN sysobjects AS so2 ON so.parent_obj = so2.Id
	WHERE [so].[type] = 'TR'
) BEGIN 
	DROP TRIGGER UpdateDateReleased
END;