-- <Migration ID="761427a3-7fab-4c76-bd5c-2e434f59e14e" />
GO

PRINT N'Creating [dbo].[GameResult]'
GO
CREATE TABLE [dbo].[GameResult]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Victory] [nvarchar] (60) NOT NULL,
[Defeat] [nvarchar] (60) NOT NULL,
[Draw] [nvarchar] (60) NOT NULL
)
GO
PRINT N'Creating primary key [PK_GameResult] on [dbo].[GameResult]'
GO
ALTER TABLE [dbo].[GameResult] ADD CONSTRAINT [PK_GameResult] PRIMARY KEY CLUSTERED  ([Id])
GO
PRINT N'Creating [dbo].[GameHistory]'
GO
CREATE TABLE [dbo].[GameHistory]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[GameTime] [datetime] NOT NULL,
[WithWhom] [nvarchar] (60) NOT NULL,
[IdResult] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_GameHistory] on [dbo].[GameHistory]'
GO
ALTER TABLE [dbo].[GameHistory] ADD CONSTRAINT [PK_GameHistory] PRIMARY KEY CLUSTERED  ([Id])
GO
PRINT N'Adding foreign keys to [dbo].[GameHistory]'
GO
ALTER TABLE [dbo].[GameHistory] ADD CONSTRAINT [FK_GameHistory_GameResult] FOREIGN KEY ([IdResult]) REFERENCES [dbo].[GameResult] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
GO
