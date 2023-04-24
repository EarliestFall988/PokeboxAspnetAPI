CREATE OR ALTER PROCEDURE Pokebox.SelectSinglePokeOwned
    @Username NVARCHAR(64),
    @PokemonName NVARCHAR(64),
    @Name NVARCHAR(64)
AS

SELECT PO.PokeOwnedID, PO.UserID, PO.PokemonID, PO.[Name], PO.DatePutInBox, PO.Gender, PO.[Level]
FROM Pokebox.[User] U
    INNER JOIN Pokebox.PokeOwned PO ON PO.UserID = U.UserID
    INNER JOIN Pokebox.Pokemon P ON P.PokemonID = PO.PokemonID
WHERE U.Username = @Username AND P.PokemonName = @PokemonName AND PO.[Name] = @Name