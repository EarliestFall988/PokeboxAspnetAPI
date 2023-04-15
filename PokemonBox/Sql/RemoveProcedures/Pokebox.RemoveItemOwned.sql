CREATE OR ALTER PROCEDURE Pokebox.RemoveItemOwned
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

DELETE FROM Pokebox.ItemOwned
WHERE UserID = @UserID AND ItemID = @ItemID
GO
