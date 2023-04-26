CREATE OR ALTER PROCEDURE Pokebox.FetchSinglePokeOwned
    @Username NVARCHAR(64),
    @PokeOwnedID INT
AS

SELECT P.PokemonName, PO.[Name]
FROM Pokebox.[User] U
    INNER JOIN Pokebox.PokeOwned PO ON PO.UserID = U.UserID
    INNER JOIN Pokebox.Pokemon P ON P.PokemonID = PO.PokemonID
WHERE U.Username = @Username AND PO.PokeOwnedID = @PokeOwnedID
GO