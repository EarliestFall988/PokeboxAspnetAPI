CREATE OR ALTER PROCEDURE Pokebox.SelectSingleItemOwned
    @Username NVARCHAR(64),
    @ItemName NVARCHAR(64)
AS

SELECT I.ItemName, I.ItemImageLink, I.[Description], IT.ItemTypeName, IOW.DatePutInBox, IOW.ItemID, IOW.UserID, IOW.ItemOwnedID
FROM [User] U 
    INNER JOIN ItemOwned IOW ON IOW.UserID = U.UserID
    INNER JOIN Item I ON I.ItemID = IOW.ItemID
    INNER JOIN ItemType IT ON IT.ItemTypeID = I.ItemTypeID
WHERE U.Username = @Username AND I.ItemName = @ItemName
GO