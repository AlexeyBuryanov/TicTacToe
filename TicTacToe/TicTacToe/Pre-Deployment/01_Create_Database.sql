IF DB_ID(N'TicTacToe') IS NOT NULL BEGIN
	PRINT(N'Создание БД TicTacToe: БД уже существует!');
END ELSE BEGIN
	CREATE DATABASE TicTacToe;
	PRINT(N'Создание БД TicTacToe: успешно.');
END
GO