using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonRepository
    {
        IReadOnlyList<Pokemon> RetrievePokemon(uint pokemonID);

        void SavePokemon(uint pokemonID, string PokemonName, uint PokedexNumber, string decription, DateTimeOffset dateAdded, bool isLegendary);
    }
}
