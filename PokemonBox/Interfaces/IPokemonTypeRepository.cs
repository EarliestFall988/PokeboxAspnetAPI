using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokemonTypeRepository
    {
        IReadOnlyList<PokemonType> SelectPokemonType();
        PokemonType AddPokemonType(string TypeName);
    }
}
