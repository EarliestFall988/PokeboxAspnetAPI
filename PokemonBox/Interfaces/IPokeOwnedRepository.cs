using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokeOwnedRepository
    {
        IReadOnlyList<Pokemon> RetrieveAllOwnedPokemon();

        PokeOwned CreatePokeOwned(string userName, string pokeName, DateTimeOffset datePutInBox, pokeGender Gender, uint level );
    
        
    
    }
}
