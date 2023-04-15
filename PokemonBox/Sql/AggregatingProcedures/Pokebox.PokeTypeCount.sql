CREATE OR ALTER PROCEDURE Pokebox.PokeTypeCount
    @StartDate DATETIMEOFFSET,
    @FinalDate DATETIMEOFFSET
AS

SELECT PTY.PokemonTypeID, PTY.PokemonTypeName, SUM(IIF(PO.DatePutInBox BETWEEN @StartDate AND @FinalDate, 1, 0)) AS PokeTypeCount
FROM Pokebox.[User] U 
INNER JOIN Pokebox.PokeOwned PO ON PO.UserID = U.UserID
INNER JOIN Pokebox.Pokemon P ON P.PokemonID = PO.PokemonID
INNER JOIN Pokebox.PokeType PT ON PT.PokemonID = P.PokemonID
RIGHT JOIN Pokebox.PokemonType PTY ON PTY.PokemonTypeID = PT.PokemonTypeID
GROUP BY PTY.PokemonTypeID, PTY.PokemonTypeName
ORDER BY PTY.PokemonTypeID ASC
