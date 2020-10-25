CREATE TABLE [dbo].[Threads]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[ThreadTitle] NVARCHAR(256) NOT NULL,
	[LatestMessageTimestamp] DATETIME2(3) NOT NULL,
	[TimestampCreated] DATETIME2(3) NOT NULL
)
