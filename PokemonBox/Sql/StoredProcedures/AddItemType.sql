CREATE OR ALTER PROCEDURE Pokebox.AddItemType
    @TypeName NVARCHAR(32)
AS
INSERT Pokebox.ItemType(ItemTypeName)
VALUES (@TypeName);
GO