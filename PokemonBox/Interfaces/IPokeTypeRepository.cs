using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokeTypeRepository
    {

        IReadOnlyList<PokeType> SelectPokeType();
        PokeType AddPokeType(string pokemonTypeName, string pokemonName);
        
    }
}
