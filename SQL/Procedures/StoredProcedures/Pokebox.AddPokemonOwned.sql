-- Give a specified user a specified pokemon

CREATE OR ALTER PROCEDURE Pokebox.AddPokemonOwned
    @Username NVARCHAR(64),
    @PokemonName NVARCHAR(64),
    @Name NVARCHAR(64),
    @Gender NVARCHAR(1),
    @Level INT,
    @PokeOwnedID INT OUTPUT,
    @DatePutInBox DATETIMEOFFSET OUTPUT,
    @OutUserID INT OUTPUT,
    @OutPokeID INT OUTPUT
AS

DECLARE @UserID INT =
    (
        SELECT U.UserID
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @PokemonID INT = 
    (
        SELECT P.PokemonID
        FROM Pokebox.Pokemon P
        WHERE P.PokemonName = @PokemonName
    )

INSERT Pokebox.PokeOwned(UserID, PokemonID, [Name], Gender, [Level])
VALUES(@UserID, @PokemonID, @Name, @Gender, @Level);

SET @PokeOwnedID = SCOPE_IDENTITY();
SET @DatePutInBox = SYSDATETIMEOFFSET();
SET @OutUserID = @UserID;
SET @OutPokeID = @PokemonID;
GO
