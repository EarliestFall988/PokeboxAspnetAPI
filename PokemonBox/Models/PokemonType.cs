namespace PokemonBox.Models
{
    public class PokemonType
    {
        public uint PokemonTypeID { get; }

        public string PokemonTypeName { get; }

        public PokemonType(uint pokemonTypeID, string pokemonTypeName)
        { 
            PokemonTypeID = pokemonTypeID;
            PokemonTypeName = pokemonTypeName;
        }
    }
}
