CREATE TABLE dbo.users (
  [Id] INT NOT NULL IDENTITY,
  [FirstName] VARCHAR(45) NULL,
  [LastName] VARCHAR(45) NULL,
  [Username] VARCHAR(45) NULL,
  [Email] VARCHAR(45) NULL,
  [PasswordHash] VARCHAR(172) NULL,
  [PaswordSalt] VARCHAR(64) NULL,
  [TimestampCreated] DATETIME2(0) NULL,
  
  CONSTRAINT user_pk PRIMARY KEY (Id)
);
