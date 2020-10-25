CREATE TABLE [dbo].[Recepients]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[EmailId] INT NOT NULL,
	[RecepientName] NVARCHAR(128) NULL,
	[RecepientEmail] NVARCHAR(128) NOT NULL,

	CONSTRAINT recepient_email_fk FOREIGN KEY (EmailId) REFERENCES dbo.Emails(Id)
)
