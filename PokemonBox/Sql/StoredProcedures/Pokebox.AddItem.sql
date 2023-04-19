CREATE OR ALTER PROCEDURE Pokebox.AddItem
    @ItemTypeName NVARCHAR(64),
    @ItemName NVARCHAR(64),
    @Description NVARCHAR(128),
    @ItemID INT OUTPUT,
    @DateAdded DATETIMEOFFSET OUTPUT,
    @OutItemTypeID INT OUTPUT
AS
DECLARE @ItemTypeID INT =
    (
        SELECT IT.ItemTypeID
        FROM Pokebox.ItemType IT
        WHERE IT.ItemTypeName = @ItemTypeName
    )

INSERT Pokebox.Item(ItemTypeID, ItemName, [Description])
Values(@ItemTypeID, @ItemName, @Description)


SET @ItemID = SCOPE_IDENTITY();
SET @DateAdded = SYSDATETIMEOFFSET();
SET @OutItemTypeID = @ItemTypeID;
GO
