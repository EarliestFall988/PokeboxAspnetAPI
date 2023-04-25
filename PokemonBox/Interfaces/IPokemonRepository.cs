using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonRepository
    {
        IReadOnlyList<Pokemon> SelectPokemon();

        IReadOnlyDictionary<Pokemon, PokeType> PokeTypeCount();

        Pokemon AddPokemon(string pokemonName, uint pokedexNumber, string imageLink, bool isLegendary);
    }
}
