-- Get number of items in database

CREATE OR ALTER PROCEDURE Pokebox.SelectItemCount
AS

SELECT COUNT(*) AS ItemCount
FROM Pokebox.Item
GO
