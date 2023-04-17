namespace PokemonBox.Models
{
    public class ItemsOwned
    {
        public uint ItemOwnedID { get; }

        public uint UserID { get; }

        public uint ItemID { get; }

        public DateTimeOffset DatePutInBox { get; }

        public ItemsOwned(uint itemsOwnedID, uint userID, uint  itemID, DateTimeOffset datePutInBox)
        {
            ItemOwnedID = itemsOwnedID;
            UserID = userID;
            ItemID = itemID;
            DatePutInBox = datePutInBox;
        }
    }
}
