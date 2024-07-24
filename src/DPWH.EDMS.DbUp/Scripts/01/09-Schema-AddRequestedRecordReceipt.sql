--Create RequestedRecordReceipts table
IF NOT EXISTS (SELECT *	FROM sysobjects	WHERE name = N'RequestedRecordReceipts' AND xtype = 'U')
BEGIN CREATE TABLE RequestedRecordReceipts (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	RecordRequestId UNIQUEIDENTIFIER NOT NULL,	
	Filename NVARCHAR(200) NOT NULL,	
	Office NVARCHAR(50) NOT NULL,	
	DateReceived [DATETIMEOFFSET](7) NOT NULL,
	TimeReceived [DATETIMEOFFSET](7) NOT NULL,
	[FileSize] [bigint] NULL,
	Uri NVARCHAR(350) NULL,	
	Created [DATETIMEOFFSET](7) NOT NULL,
	CreatedBy NVARCHAR(150),
	LastModified [DATETIMEOFFSET](7) NULL,
	LastModifiedBy NVARCHAR(150) NULL,
);
END

-- Add Constraints
ALTER TABLE [dbo].[RequestedRecordReceipts]  WITH NOCHECK ADD CONSTRAINT [FK_RequestedRecordReceipts_RecordRequest_Id] FOREIGN KEY([RecordRequestId])
REFERENCES [dbo].[RecordRequests] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequestedRecordReceipts] CHECK CONSTRAINT [FK_RequestedRecordReceipts_RecordRequest_Id]
GO