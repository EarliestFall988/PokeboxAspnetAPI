CREATE OR ALTER PROCEDURE Pokebox.AddItem
    @ItemTypeName NVARCHAR(64),
    @ItemName NVARCHAR(64),
    @Description NVARCHAR(128)
AS
INSERT Pokebox.Item(ItemTypeID, ItemName, [Description])
SELECT IT.ItemTypeID, @ItemName, @Description
FROM ItemType IT 
WHERE IT.ItemTypeName = @ItemTypeName;
GO
