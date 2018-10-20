-- <Migration ID="ce79b2dc-991f-4ada-bfa8-f007875c823e" />
GO

PRINT N'Creating [dbo].[Clients]'
GO
CREATE TABLE [dbo].[Clients]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UserName] [nvarchar] (60) NOT NULL,
[Password] [nvarchar] (60) NOT NULL,
[Email] [nvarchar] (60) NOT NULL
)
GO
PRINT N'Creating primary key [PK_Clients] on [dbo].[Clients]'
GO
ALTER TABLE [dbo].[Clients] ADD CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED  ([Id])
GO
