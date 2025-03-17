CREATE TABLE [UserBookReviews]
(
    [BookId] UNIQUEIDENTIFIER NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Rating] INT NOT NULL,
    [Review] NVARCHAR(MAX) NULL,
    [CreatedOn] DATETIME NOT NULL,
    CONSTRAINT [PK_UserBookReviews] PRIMARY KEY ([BookId], [UserId]),
    CONSTRAINT [FK_UserBookReviews_Book] FOREIGN KEY ([BookId]) REFERENCES [Books] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserBookReviews_User] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
)