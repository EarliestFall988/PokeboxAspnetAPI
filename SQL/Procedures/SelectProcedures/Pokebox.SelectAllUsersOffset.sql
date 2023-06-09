-- Get a single page of 30 userss

CREATE OR ALTER PROCEDURE Pokebox.SelectAllUserOffset
	@Page INT
AS

SELECT U.Username
FROM Pokebox.[User] U
ORDER BY U.Username ASC
OFFSET (@Page - 1) * 30 ROWS FETCH NEXT 30 ROWS ONLY

GO