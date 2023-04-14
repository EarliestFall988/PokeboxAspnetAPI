namespace PokemonBox.Models
{
    public class PokeType
    {
        public uint PokemonID { get; }

        public uint PokemonTypeID { get; }

        public PokeType(uint pokemonID, uint pokemonTypeID)
        {
            PokemonID = pokemonID;
            PokemonTypeID = pokemonTypeID;
        }
    }
}
