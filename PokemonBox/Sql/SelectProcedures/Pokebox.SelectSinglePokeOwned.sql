CREATE OR ALTER PROCEDURE Pokebox.SelectSinglePokeOwned
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

SELECT *
FROM PokeOwned PO
WHERE PO.UserID = @UserID AND PO.PokemonID = @PokemonID AND PO.[Name] = @Name