namespace PokemonBox.Models
{
    public class TopItem
    {
        public uint ItemID { get; }

        public string ItemName { get; }

        public uint ItemCount { get; }

        public string ItemImageLink { get; }

        public uint Rank { get; }

        public TopItem(uint itemID, string itemName, uint itemCount, string itemImageLink, uint rank)
        {
            ItemID = itemID;
            ItemName = itemName;
            ItemCount = itemCount;
            ItemImageLink = itemImageLink;
            Rank = rank;    
        }
    }
}
