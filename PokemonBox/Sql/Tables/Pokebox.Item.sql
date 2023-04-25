CREATE TABLE Pokebox.Item
(
    ItemID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    ItemTypeID INT NOT NULL FOREIGN KEY
        REFERENCES Pokebox.ItemType(ItemTypeID),
    ItemName NVARCHAR(64) NOT NULL,
    [Description] NVARCHAR(256) NOT NULL,
    DateAdded DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    ItemImageLink NVARCHAR(256) NULL DEFAULT(N''),

    UNIQUE(ItemName)
)