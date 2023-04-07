namespace PokemonBox.Models
{
    public class ItemsOwned
    {
        public uint ItemOwnedID { get; }

        public uint ItemsBoxID { get; }

        public uint ItemID { get; }

        public DateTimeOffset DatePutInBox { get; }

        public ItemsOwned(uint itemsOwnedID, uint itemsBoxID, uint  itemID, DateTimeOffset datePutInBox)
        {
            ItemOwnedID = itemsOwnedID;
            ItemsBoxID = itemsBoxID;
            ItemID = itemID;
            DatePutInBox = datePutInBox;
        }
    }
}
