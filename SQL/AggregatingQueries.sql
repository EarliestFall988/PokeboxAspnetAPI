CREATE OR ALTER PROCEDURE Pokebox.TopItem
    @Month INT,
    @Year INT
AS

SELECT I.ItemID, ItemName, COUNT(*) AS ItemCount
FROM Pokebox.[User] U
INNER JOIN Pokebox.ItemOwned IOW ON IOW.UserID = U.UserID
INNER JOIN Pokebox.Item I ON I.ItemID = IOW.ItemID
WHERE MONTH(IOW.DatePutInBox) = @Month AND YEAR(IOW.DatePutInBox) = @Year
GROUP BY I.ItemID, I.ItemName
ORDER BY ItemCount DESC, I.ItemID ASC

GO


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

GO

CREATE OR ALTER PROCEDURE Pokebox.PokeRank
    @PokemonName NVARCHAR(64)
AS

SELECT U.UserID, U.Username, SUM(IIF(P.PokemonName = @PokemonName, 1, 0)) AS PokemonCount, 
    RANK() OVER (ORDER BY SUM(IIF(P.PokemonName = @PokemonName, 1, 0))DESC) AS Rank
FROM Pokebox.Pokemon P
    INNER JOIN Pokebox.PokeOwned PO ON PO.PokemonID = P.PokemonID
    RIGHT JOIN Pokebox.[User] U ON U.UserID = PO.UserID
GROUP BY U.UserID, U.Username
ORDER BY Rank

GO

CREATE OR ALTER PROCEDURE Pokebox.AverageLevel
AS

SELECT U.UserID, U.Username, AVG(PO.[Level]) AS AveragePokeLevel
FROM Pokebox.[User] U 
INNER JOIN Pokebox.PokeOwned PO ON U.UserID = PO.UserID
GROUP BY U.UserID, U.Username
