CREATE TABLE [Tags]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newId(),
	[Name] varchar(200) NOT NULL,
);