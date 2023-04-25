namespace PokemonBox.Models
{
    public class Pokemon
    {
        public uint PokemonID { get; }

        public string PokemonName { get; }

        public uint PokedexNumber { get; }

        public string ImageLink { get; }

        public DateTimeOffset DateAdded { get; }

        public bool IsLegendary { get; }


        public Pokemon(uint pokemonID, string pokemonName, uint pokedexNumber, string imageLink,
            DateTimeOffset dateAdded, bool isLegendary) {
            PokemonID = pokemonID;
            PokemonName = pokemonName;
            PokedexNumber = pokedexNumber;
            ImageLink = imageLink;
            DateAdded = dateAdded;
            IsLegendary = isLegendary;
        }
    }
}
