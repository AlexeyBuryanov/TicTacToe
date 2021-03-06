﻿-- <Migration ID="dc791d60-73af-4f3b-9896-23af6fb4fe89" />
GO

PRINT N'Dropping foreign keys from [dbo].[GameHistory]'
GO
ALTER TABLE [dbo].[GameHistory] DROP CONSTRAINT [FK_GameHistory_Clients]
GO
ALTER TABLE [dbo].[GameHistory] DROP CONSTRAINT [FK_GameHistory_GameResult]
GO
PRINT N'Dropping constraints from [dbo].[GameHistory]'
GO
ALTER TABLE [dbo].[GameHistory] DROP CONSTRAINT [PK_GameHistory]
GO
PRINT N'Rebuilding [dbo].[GameHistory]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_GameHistory]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[GameTime] [datetime] NOT NULL,
[Who] [nvarchar] (60) NOT NULL,
[WithWhom] [nvarchar] (60) NOT NULL,
[IdResult] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_GameHistory] ON
GO
INSERT INTO [dbo].[RG_Recovery_1_GameHistory]([Id], [GameTime], [WithWhom], [IdResult]) SELECT [Id], [GameTime], [WithWhom], [IdResult] FROM [dbo].[GameHistory]
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_GameHistory] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[GameHistory]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[RG_Recovery_1_GameHistory]', RESEED, @idVal)
GO
DROP TABLE [dbo].[GameHistory]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_GameHistory]', N'GameHistory', N'OBJECT'
GO
PRINT N'Creating primary key [PK_GameHistory] on [dbo].[GameHistory]'
GO
ALTER TABLE [dbo].[GameHistory] ADD CONSTRAINT [PK_GameHistory] PRIMARY KEY CLUSTERED  ([Id])
GO
PRINT N'Adding foreign keys to [dbo].[GameHistory]'
GO
ALTER TABLE [dbo].[GameHistory] ADD CONSTRAINT [FK_GameHistory_GameResult] FOREIGN KEY ([IdResult]) REFERENCES [dbo].[GameResult] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
GO
