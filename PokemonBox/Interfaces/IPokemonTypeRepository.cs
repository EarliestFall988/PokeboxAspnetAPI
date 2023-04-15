using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonTypeRepository
    {
        IReadOnlyList<PokemonType> SelectPokemonTypes();
        PokemonType AddPokemonType(string TypeName);
    }
}
