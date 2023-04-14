using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonRepository
    {
        IReadOnlyList<Pokemon> SelectPokemon();
        
        //TODO: No procedure made for 
        //Pokemon GetPokemon(string pokemonName); //using unique key
        //TODO: No procedure made for 
        //Pokemon FetchPokemon(uint pokemonID); //prim key

        Pokemon AddPokemon(string pokemonName, uint pokedexNumber, string decription, bool isLegendary);
    }
}
