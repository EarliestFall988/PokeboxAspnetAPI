-- Get a page of pokemon for a user

CREATE OR ALTER PROCEDURE Pokebox.SelectAllPokeOwnedOffset
    @Username NVARCHAR(64),
    @Page INT
AS

SELECT PO.[Name], PO.[Level], PO.Gender, P.PokemonName, POT.PokemonTypeName, P.IsLegendary, PO.DatePutInBox,
    U.Username, PO.PokemonID, PO.PokeOwnedID, P.ImageLink
FROM [User] U
    INNER JOIN PokeOwned PO ON PO.UserID = U.UserID
    INNER JOIN Pokemon P ON P.PokemonID = PO.PokemonID
    INNER JOIN PokeType PT ON PT.PokemonID = P.PokemonID
    INNER JOIN PokemonType POT ON POT.PokemonTypeID = PT.PokemonTypeID 
WHERE U.Username = @Username
ORDER BY PO.PokemonID ASC
OFFSET (@Page - 1) * 30 ROWS FETCH NEXT 30 ROWS ONLY

GO