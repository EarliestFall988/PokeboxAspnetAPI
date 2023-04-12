using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonRepository
    {
        IReadOnlyList<Pokemon> RetrievePokemon();

        Pokemon GetPokemon(string pokemonName); //using unique key

        Pokemon FetchPokemon(uint pokemonID); //prim key

        Pokemon AddPokemon(string pokemonName, uint pokedexNumber, string decription, bool isLegendary);
    }
}
