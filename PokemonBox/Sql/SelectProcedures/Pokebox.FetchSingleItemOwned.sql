CREATE OR ALTER PROCEDURE Pokebox.FetchSingleItemOwned
    @Username NVARCHAR(64),
    @ItemOwnedID INT
AS

SELECT I.ItemID
FROM [User] U 
    INNER JOIN ItemOwned IOW ON IOW.UserID = U.UserID
    INNER JOIN Item I ON I.ItemID = IOW.ItemID
    INNER JOIN ItemType IT ON IT.ItemTypeID = I.ItemTypeID
WHERE U.Username = @Username AND IOW.ItemOwnedID = @ItemOwnedID
GO