CREATE OR ALTER PROCEDURE Pokebox.SelectAllPokemonOwnedByUser
    @Username NVARCHAR(32)
AS

SELECT PO.[Name], PO.[Level], PO.Gender, P.PokemonName, POT.PokemonTypeName, P.IsLegendary, PO.DatePutInBox
FROM [User] U
    INNER JOIN PokeOwned PO ON PO.UserID = U.UserID
    INNER JOIN Pokemon P ON P.PokemonID = PO.PokemonID
    INNER JOIN PokeType PT ON PT.PokemonID = P.PokemonID
    INNER JOIN PokemonType POT ON POT.PokemonTypeID = PT.PokemonTypeID 
WHERE U.Username = @Username
GO
