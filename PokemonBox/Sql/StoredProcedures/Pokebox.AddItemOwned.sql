CREATE OR ALTER PROCEDURE Pokebox.AddItemOwned
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
        WHERE I.ItemName = @ItemName
    )

INSERT Pokebox.ItemOwned(UserID, ItemID)
VALUES(@UserID, @ItemID);
GO
