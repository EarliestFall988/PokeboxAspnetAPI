using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokeOwnedRepository
    {
        PokeOwned CreatePokeOwned(string userName, string pokemonName, string name, pokeGender gender, uint level );

        void RemovePokeOwned(string userName, string pokemonName, string pokeName);

        IReadOnlyList<PokeOwned> SelectAllPokemonOwnedByUser(string userName);
    
    }
}
