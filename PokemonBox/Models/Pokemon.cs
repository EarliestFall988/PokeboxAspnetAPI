namespace PokemonBox.Models
{
    public class Pokemon
    {
        public uint PokemonID { get; }

        public string PokemonName { get; }

        public uint PokedexNumber { get; }

        public string Decription { get; }

        public DateTimeOffset DateAdded { get; }

        public bool IsLegendary { get; }


        public Pokemon(uint pokemonID, string pokemonName, uint pokedexNumber, string decription,
            DateTimeOffset dateAdded, bool isLegendary) {
            PokemonID = pokemonID;
            PokemonName = pokemonName;
            PokedexNumber = pokedexNumber;
            Decription = decription;
            DateAdded = dateAdded;
            IsLegendary = isLegendary;
        }
    }
}
