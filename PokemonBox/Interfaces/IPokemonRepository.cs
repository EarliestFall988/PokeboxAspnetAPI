using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonRepository
    {
        IReadOnlyList<Pokemon> SelectPokemon();
        
        Pokemon AddPokemon(string pokemonName, uint pokedexNumber, string imageLink, bool isLegendary);
    }
}
