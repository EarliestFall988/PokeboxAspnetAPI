using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokeOwnedRepository
    {
        IReadOnlyList<Pokemon> RetrieveAllOwnedPokemon();

        PokeOwned CreatePokeOwned(uint userID, uint pokemonID, string pokeName, DateTimeOffset datePutInBox, pokeGender Gender, uint level );
    
        
    
    }
}
