CREATE TABLE [Chapters]
(
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newId() NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedOn] DATETIME2(7) NOT NULL,
    [LastEditedOn] DATETIME2(7) NOT NULL,
    [ReleasedOn] DATETIME2(7) NULL,
    [BookId] UNIQUEIDENTIFIER NOT NULL,
    [AuthorNoteTop] NVARCHAR(350) NULL,
    [AuthorNoteBottom] NVARCHAR(350) NULL,
    [Name] NVARCHAR(255) NOT NULL,

    CONSTRAINT [FK_BookId_Book] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id]) ON DELETE CASCADE
);