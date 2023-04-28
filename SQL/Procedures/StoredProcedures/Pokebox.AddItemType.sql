-- Add a new item type

CREATE OR ALTER PROCEDURE Pokebox.AddItemType
    @TypeName NVARCHAR(32),
    @OutItemTypeID INT OUTPUT
AS
INSERT Pokebox.ItemType(ItemTypeName)
VALUES (@TypeName);
SET @OutItemTypeID = SCOPE_IDENTITY();
GO