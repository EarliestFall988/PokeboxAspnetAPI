namespace PokemonBox.Models
{
    public class Item
    {
        public uint ItemID { get; }

        public uint ItemTypeID { get; }

        public string ItemName { get; }

        public DateTimeOffset DateAdded { get; }

        public string Description { get; }

        public Item(uint itemID, uint itemTypeID, string itemName, DateTimeOffset dateAdded, string description)
        {
            ItemID = itemID;
            ItemTypeID = itemTypeID;
            ItemName = itemName;
            DateAdded = dateAdded;
            Description = description;
        }
    }
}
