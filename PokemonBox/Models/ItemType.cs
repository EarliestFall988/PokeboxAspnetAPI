namespace PokemonBox.Models
{
    public class ItemType
    {
        public uint ItemTypeID { get; }

        public string ItemTypeName { get; }

        public ItemType(uint itemTypeID, string itemTypeName)
        {
            ItemTypeID = itemTypeID;
            ItemTypeName = itemTypeName;
        }
    }
}
