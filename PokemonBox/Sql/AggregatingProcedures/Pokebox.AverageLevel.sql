CREATE OR ALTER PROCEDURE Pokebox.AverageLevel
@OutAverage DOUBLE PRECISION OUTPUT
AS
DECLARE @AveragePokeLevel DOUBLE PRECISION = 
	(
		SELECT AVG(PO.[Level])
		FROM Pokebox.[User] U 
		INNER JOIN Pokebox.PokeOwned PO ON U.UserID = PO.UserID
		GROUP BY U.UserID, U.Username
	)

SET @OutAverage = @AveragePokeLevel;
GO
