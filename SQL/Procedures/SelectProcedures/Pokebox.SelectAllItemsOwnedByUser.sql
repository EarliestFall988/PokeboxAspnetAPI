-- Get all items owned by user

CREATE OR ALTER PROCEDURE Pokebox.SelectAllItemsOwnedByUser
    @Username NVARCHAR(64)
AS

SELECT I.ItemName, IT.ItemTypeName, IOW.DatePutInBox, IOW.ItemOwnedID, U.UserID, I.ItemID
FROM [User] U 
    INNER JOIN ItemOwned IOW ON IOW.UserID = U.UserID
    INNER JOIN Item I ON I.ItemID = IOW.ItemID
    INNER JOIN ItemType IT ON IT.ItemTypeID = I.ItemTypeID
WHERE U.Username = @Username
GO