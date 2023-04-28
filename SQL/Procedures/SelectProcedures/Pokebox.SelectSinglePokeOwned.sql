-- Get a single owned pokemon

CREATE OR ALTER PROCEDURE Pokebox.SelectSinglePokeOwned
    @Username NVARCHAR(64),
    @PokemonName NVARCHAR(64),
    @Name NVARCHAR(64)
AS

SELECT PO.PokeOwnedID, PO.UserID, PO.PokemonID, PO.[Name], PO.DatePutInBox, PO.Gender, PO.[Level], P.ImageLink, POT.PokemonTypeName
FROM Pokebox.[User] U
    INNER JOIN Pokebox.PokeOwned PO ON PO.UserID = U.UserID
    INNER JOIN Pokebox.Pokemon P ON P.PokemonID = PO.PokemonID
    INNER JOIN Pokebox.PokeType PT ON PT.PokemonID = P.PokemonID
    INNER JOIN Pokebox.PokemonType POT ON POT.PokemonTypeID = PT.PokemonTypeID
WHERE U.Username = @Username AND P.PokemonName = @PokemonName AND PO.[Name] = @Name