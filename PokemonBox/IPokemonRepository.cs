using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonRepository
    {
        IReadOnlyList<Pokemon> RetrievePokemon();

        Pokemon GetPokemon(string pokemonName);

        Pokemon FetchPokemon(uint pokemonID);

        Pokemon CreatePokemon(string pokemonName, uint pokedexNumber, string decription, DateTimeOffset dateAdded, bool isLegendary);
    }
}
