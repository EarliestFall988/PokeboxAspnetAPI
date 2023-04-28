-- Add a new item into the Items

CREATE OR ALTER PROCEDURE Pokebox.AddItem
    @ItemTypeName NVARCHAR(64),
    @ItemName NVARCHAR(64),
    @Description NVARCHAR(128),
	@ItemImageLink NVARCHAR(256),
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

INSERT Pokebox.Item(ItemTypeID, ItemName, [Description], ItemImageLink)
Values(@ItemTypeID, @ItemName, @Description, @ItemImageLink)


SET @ItemID = SCOPE_IDENTITY();
SET @DateAdded = SYSDATETIMEOFFSET();
SET @OutItemTypeID = @ItemTypeID;
GO
