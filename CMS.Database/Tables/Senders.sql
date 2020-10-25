CREATE TABLE [dbo].[Senders]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[EmailId] INT NOT NULL,
	[SenderName] NVARCHAR(128) NULL,
	[SenderEmail] NVARCHAR(128) NOT NULL,

	CONSTRAINT sender_email_fk FOREIGN KEY (EmailId) REFERENCES dbo.Emails(Id)

)
