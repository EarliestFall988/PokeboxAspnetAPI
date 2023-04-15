CREATE OR ALTER PROCEDURE Pokebox.RemovePokeOwned
    @Username NVARCHAR(64),
    @PokemonName NVARCHAR(64),
    @Name NVARCHAR(64)
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

DELETE FROM Pokebox.PokeOwned
WHERE UserID = @UserID AND [PokemonID] = @PokemonID AND [Name] = @Name

GO
