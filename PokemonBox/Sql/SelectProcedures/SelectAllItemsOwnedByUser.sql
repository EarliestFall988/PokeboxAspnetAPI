CREATE OR ALTER PROCEDURE Pokebox.SelectAllItemsOwnedByUser
    @Username NVARCHAR(32)
AS

SELECT I.ItemName, IT.ItemTypeName, IOW.DatePutInBox
FROM [User] U 
    INNER JOIN ItemOwned IOW ON IOW.UserID = U.UserID
    INNER JOIN Item I ON I.ItemID = IOW.ItemID
    INNER JOIN ItemType IT ON IT.ItemTypeID = I.ItemTypeID
WHERE U.Username = @Username
GO