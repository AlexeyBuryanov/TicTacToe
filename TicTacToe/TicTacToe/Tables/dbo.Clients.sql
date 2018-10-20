CREATE TABLE Clients( -- КЛИЕНТЫ
	Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clients PRIMARY KEY,
	UserName   NVARCHAR(60)      NOT NULL,
	[Password] NVARCHAR(60)      NOT NULL,
	Email      NVARCHAR(60)      NOT NULL
);