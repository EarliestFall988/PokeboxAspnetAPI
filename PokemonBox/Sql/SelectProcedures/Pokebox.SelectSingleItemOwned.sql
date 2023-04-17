CREATE OR ALTER PROCEDURE Pokebox.SelectSingleItemOwned
    @Username NVARCHAR(64),
    @ItemName NVARCHAR(64)
AS

DECLARE @UserID INT =
    (
        SELECT U.UserID
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @ItemID INT = 
    (
        SELECT I.ItemID
        FROM Pokebox.Item I
        WHERE ItemName = @ItemName
    )

SELECT *
FROM Pokebox.ItemOwned I
WHERE I.UserID = @UserID AND I.ItemID = @ItemID