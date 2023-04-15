CREATE OR ALTER PROCEDURE Pokebox.TopItem
    @Month INT,
    @Year INT
AS

SELECT I.ItemID, ItemName, COUNT(*) AS ItemCount
FROM Pokebox.[User] U
INNER JOIN Pokebox.ItemOwned IOW ON IOW.UserID = U.UserID
INNER JOIN Pokebox.Item I ON I.ItemID = IOW.ItemID
WHERE MONTH(IOW.DatePutInBox) = @Month AND YEAR(IOW.DatePutInBox) = @Year
GROUP BY I.ItemID, I.ItemName
ORDER BY ItemCount DESC, I.ItemID ASC
