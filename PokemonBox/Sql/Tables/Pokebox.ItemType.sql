CREATE TABLE Pokebox.ItemType
(
    ItemTypeID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    ItemTypeName NVARCHAR(64) NOT NULL,

    UNIQUE(ItemTypeName)
)