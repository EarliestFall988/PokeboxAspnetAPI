namespace PokemonBox.Models
{
    public class TopItem
    {
        public uint ItemID { get; }

        public string ItemName { get; }

        public uint ItemCount { get; }

        public string ItemImageLink { get; }

        public TopItem(uint itemID, string itemName, uint itemCount, string itemImageLink)
        {
            ItemID = itemID;
            ItemName = itemName;
            ItemCount = itemCount;
            ItemImageLink = itemImageLink;
        }
    }
}
