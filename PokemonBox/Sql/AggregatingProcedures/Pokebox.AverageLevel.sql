CREATE OR ALTER PROCEDURE Pokebox.AverageLevel
AS

SELECT U.UserID, U.Username, AVG(PO.[Level]) AS AveragePokeLevel
FROM Pokebox.[User] U 
INNER JOIN Pokebox.PokeOwned PO ON U.UserID = PO.UserID
GROUP BY U.UserID, U.Username
