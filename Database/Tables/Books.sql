CREATE TABLE [Books]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newId() NOT NULL, 
    [Title] NVARCHAR(255) NOT NULL, 
    [ReleaseDate] DATETIME2 NOT NULL, 
    [AuthorId] UNIQUEIDENTIFIER NULL, 
    [Description] nvarchar(MAX) NULL,
    [Image] nvarchar(MAX) NULL,
    
    CONSTRAINT [FK_AuthorId_User] FOREIGN KEY ([AuthorId]) REFERENCES [Users]([Id]) ON DELETE SET NULL
);