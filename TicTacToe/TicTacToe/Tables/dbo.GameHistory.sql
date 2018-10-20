CREATE TABLE GameHistory( -- ИСТОРИЯ ИГР
	Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_GameHistory PRIMARY KEY,
	GameTime   DATETIME          NOT NULL,                         -- время игры
	Who        NVARCHAR(60)      NOT NULL,                         -- кто
	WithWhom   NVARCHAR(60)      NOT NULL,                         -- с кем была игра                    
	IdResult   INT NOT NULL CONSTRAINT FK_GameHistory_GameResult   -- результат игры
			   FOREIGN KEY REFERENCES GameResult(Id)
			   ON UPDATE CASCADE
			   ON DELETE CASCADE
);
GO

CREATE TABLE GameResult( -- РЕЗУЛЬТАТ ИГРЫ
	Id        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_GameResult PRIMARY KEY,
	[State]   NVARCHAR(60)      NOT NULL
);
GO