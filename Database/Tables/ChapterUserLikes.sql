CREATE TABLE [ChapterUserLikes]
(
    [ChapterId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ChapterUserLikes] PRIMARY KEY ([ChapterId], [UserId]),
    CONSTRAINT [FK_ChapterUserLikes_Chapter] FOREIGN KEY ([ChapterId]) REFERENCES [Chapters] ([Id]) ON DELETE CASCADE,
    Constraint [FK_ChapterUserLikes_User] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
)