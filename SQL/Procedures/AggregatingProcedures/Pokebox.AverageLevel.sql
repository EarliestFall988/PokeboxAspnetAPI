-- Get the average level of all user

CREATE OR ALTER PROCEDURE Pokebox.AverageLevel
AS

SELECT U.UserID, U.Username, CAST(AVG(PO.[Level]) AS DECIMAL(10,2)) AS AveragePokeLevel
FROM Pokebox.[User] U 
INNER JOIN Pokebox.PokeOwned PO ON U.UserID = PO.UserID
GROUP BY U.UserID, U.Username

GO
