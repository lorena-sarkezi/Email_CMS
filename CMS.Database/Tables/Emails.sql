CREATE TABLE [dbo].[Emails]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[ThreadId] INT,
	[Subject] NVARCHAR(256) NULL,
	[IsIncoming] BIT NOT NULL DEFAULT NULL,
	[HtmlContent] NVARCHAR(MAX) NULL,
	[TextContent] NVARCHAR(MAX) NULL,
	[MessageContent] NVARCHAR(MAX) NULL,

	CONSTRAINT thread_fk FOREIGN KEY (ThreadId) REFERENCES dbo.Threads(Id)
)