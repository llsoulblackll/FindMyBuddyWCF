USE master
GO

CREATE DATABASE FindMyBuddy
ON PRIMARY (
	NAME = FindMyBuddy_Data,
	FILENAME = 'C:\database\FindMyBuddy_Data.mdf',
	SIZE = 5MB,
	FILEGROWTH = 5MB
) LOG ON (
	NAME = FindMyBuddy_Log,
	FILENAME = 'C:\database\FindMyBuddy_Log.ldf',
	SIZE = 1mb,
	FILEGROWTH = 5MB
)
GO

USE FindMyBuddy
GO

CREATE TABLE Pet
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(200),
	Age INT,
	Description VARCHAR(MAX),
	ImagePath VARCHAR(MAX),
	LostLat FLOAT,--MAPS TO DOUBLE
	LostLon FLOAT
)
GO

ALTER TABLE Pet ADD Race VARCHAR(200)
ALTER TABLE Pet ADD Size VARCHAR(200)
GO

SP_COLUMNS PET
GO

CREATE PROC usp_Pet_Insert
@name VARCHAR(200),
@age INT,
@description VARCHAR(MAX),
@imagePath VARCHAR(MAX),
@lostLat FLOAT,
@lostLon FLOAT,
@race VARCHAR(200),
@size VARCHAR(200),
@id INT OUT
AS
INSERT INTO Pet(Name, Age, Description, ImagePath, LostLat, LostLon, Race, Size) 
VALUES(@name, @age, @description, @imagePath, @lostLat, @lostLon, @race, @size)

SET @id = SCOPE_IDENTITY()
GO

CREATE PROCEDURE usp_Pet_Update
@id INT,
@name VARCHAR(200),
@age INT,
@description VARCHAR(MAX),
@imagePath VARCHAR(MAX),
@lostLat FLOAT,
@lostLon FLOAT,
@race VARCHAR(200),
@size VARCHAR(200)
AS
UPDATE PET SET Name = @name, 
			   Age = @age, 
			   Description = @description, 
			   ImagePath = @imagePath, 
			   LostLat = @lostLat,
			   LostLon = @lostLon, 
			   Race = @race,
			   Size = @size
WHERE Id = @id
GO

CREATE PROCEDURE usp_Pet_Find
@id INT
AS
SELECT Id, Name, Age, Description, ImagePath, LostLat, LostLon, Race, Size
FROM Pet WHERE Id = @id
GO

CREATE PROCEDURE usp_Pet_FindAll
AS
SELECT Id, Name, Age, Description, ImagePath, LostLat, LostLon, Race, Size
FROM Pet
GO

CREATE PROCEDURE usp_Pet_Delete
@id INT
AS
DELETE FROM Pet WHERE Id = @id
GO


EXEC usp_Pet_Insert 'Bobby', 12, 'Brown Litte dog', 'path', 32.2, 442.3, 'HotDog', 'Small'

DECLARE @id INT
EXEC usp_Pet_Insert 'Brad', 3, 'Furry', '', 42.1, 321.3, 'BullDog', 'Big Enough', @id OUT
SELECT @id

EXEC usp_Pet_FindAll

EXEC SP_STORED_PROCEDURES '%', 'dbo'

SELECT * FROM INFORMATION_SCHEMA.ROUTINES

EXEC SP_HELP Pet


