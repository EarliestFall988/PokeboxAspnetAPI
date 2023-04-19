CREATE OR ALTER PROCEDURE Pokebox.SelectSingleUser
    @Username NVARCHAR(64),
	@OutUserID NVARCHAR(64) OUTPUT,
	@OutPassword NVARCHAR(64) OUTPUT,
	@OutFirstName NVARCHAR(64) OUTPUT,
	@OutLastName NVARCHAR(64) OUTPUT,
	@OutIsAdmin INT OUTPUT,
	@OutDateCreated DATETIMEOFFSET OUTPUT
AS

DECLARE @UserID INT =
    (
        SELECT U.UserID
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @Password NVARCHAR(64) =
    (
        SELECT U.[Password]
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @FirstName NVARCHAR(64) =
    (
        SELECT U.FirstName
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @LastName NVARCHAR(64) =
    (
        SELECT U.LastName
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @IsAdmin NVARCHAR(64) =
    (
        SELECT U.IsAdmin
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @DateCreated DATETIMEOFFSET =
    (
        SELECT U.DateCreated
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

SELECT *
FROM Pokebox.[User] U
WHERE U.Username = @Username

SET @OutUserID = @UserID;
SET @OutPassword = @Password;
SET @OutFirstName = @FirstName;
SET @OutLastName = @LastName;
SET @OutIsAdmin = @IsAdmin;
SET @OutDateCreated = @DateCreated;