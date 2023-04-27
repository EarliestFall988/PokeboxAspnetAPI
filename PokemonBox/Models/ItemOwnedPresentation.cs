namespace PokemonBox.Models
{
    public class ItemOwnedPresentation
    {
        public DateTimeOffset DateTimeOffset { get; }

        public string ItemName { get; }

        public string ItemTypeName { get; }

        public string ItemImageLink { get; }

        public string Description { get; }

        public ItemOwnedPresentation()
        {
            DateTimeOffset = DateTimeOffset.Now;
            ItemName = "ItemName";
            ItemTypeName = "ItemTypeName";
            ItemImageLink = "ItemImageLink";
            Description = "Description";
        }

        public ItemOwnedPresentation(DateTimeOffset dateTimeOffset, string itemName, string itemTypeName, string itemImageLink, string description)
        {
            DateTimeOffset = dateTimeOffset;
            ItemName = itemName;
            ItemTypeName = itemTypeName;
            ItemImageLink = itemImageLink;
            Description = description;
        }
    }
}
