-- Get a page of items owned by a user for the website

CREATE OR ALTER PROCEDURE Pokebox.SelectAllItemOwnedOffset
    @Username NVARCHAR(64),
    @Page INT
AS

SELECT ITO.DatePutInBox, IT.ItemTypeName, I.ItemImageLink, I.ItemName, I.[Description] --, ITO.ItemOwnedID, ITO.UserID, ITO.ItemID
FROM [User] U
    INNER JOIN ItemOwned ITO ON ITO.UserID = U.UserID
    INNER JOIN Item I ON I.ItemID = ITO.ItemID
    INNER JOIN ItemType IT ON IT.ItemTypeID = I.ItemTypeID 
WHERE U.Username = @Username
ORDER BY I.ItemID ASC
OFFSET (@Page - 1) * 30 ROWS FETCH NEXT 30 ROWS ONLY

GO