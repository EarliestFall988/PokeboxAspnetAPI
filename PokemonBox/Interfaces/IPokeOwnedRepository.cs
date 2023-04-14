using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokeOwnedRepository
    {
        PokeOwned CreatePokeOwned(string userName, string pokemonName, string name, pokeGender gender, uint level );

        void RemovePokeOwned(uint userID, uint pokemonID, string pokeName);
    
    }
}
