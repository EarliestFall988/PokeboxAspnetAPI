-- Add a new item owned for a user

CREATE OR ALTER PROCEDURE Pokebox.AddItemOwned
    @Username NVARCHAR(64),
    @ItemName NVARCHAR(64),
    @OutUserID INT OUTPUT,
    @OutItemID INT OUTPUT,
    @DatePutInBox DATETIMEOFFSET OUTPUT,
    @ItemOwnedID INT OUTPUT
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

SET @OutUserID = @UserID;
SET @OutItemID = @ItemID;
SET @DatePutInBox = SYSDATETIMEOFFSET();
SET @ItemOwnedID = SCOPE_IDENTITY();
GO
