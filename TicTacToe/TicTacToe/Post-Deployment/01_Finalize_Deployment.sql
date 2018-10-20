/*
Post-Deployment Script Template
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:      :r .\myfile.sql
 Use SQLCMD syntax to reference a variable in the post-deployment script.
 Example:      :setvar TableName MyTable
               SELECT * FROM [$(TableName)]
--------------------------------------------------------------------------------------
*/
INSERT INTO Clients (UserName, [Password], Email) VALUES
	('Admin', 'root', 'admin@root.db'),
	('test', 'test', 'test@test.te');
GO

INSERT INTO GameResult ([State]) VALUES
	('Victory'), -- победа
	('Defeat'),	 -- поражение
	('Draw')	 -- ничья
GO

INSERT INTO GameHistory (GameTime, Who, WithWhom, IdResult) VALUES
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1),
	('01.01.2012 20:00', 'Admin', 'Bot', 1);
GO