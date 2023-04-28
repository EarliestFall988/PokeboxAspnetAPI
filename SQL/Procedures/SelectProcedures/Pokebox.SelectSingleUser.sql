-- Get a single user

CREATE OR ALTER PROCEDURE Pokebox.SelectSingleUser
    @Username NVARCHAR(64)
AS

SELECT *
FROM Pokebox.[User] U
WHERE U.Username = @Username