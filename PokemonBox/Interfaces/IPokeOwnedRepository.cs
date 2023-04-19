﻿using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IPokeOwnedRepository
    {
        PokeOwned CreatePokeOwned(string userName, string pokemonName, string name, pokeGender gender, uint level );

        void RemovePokeOwned(string userName, string pokemonName, string pokeName);

        IReadOnlyList<PokeOwned> SelectAllPokemonOwnedByUser(string userName);

        IReadOnlyList<PokeOwned> SelectAllPokemonOwned();

        PokeOwned SelectSinglePokeOwned(string userName, string pokemonName, string nickName);

        /// <summary>
        /// Gets the average level of pokemon each user has 
        /// </summary>
        /// <returns>The dicitionary uint is UserID and decimal is the average</returns>
        IReadOnlyDictionary<uint, decimal> AverageLevel();

        /// <summary>
        /// Gets the number of a certain pokemon that the user has
        /// </summary>
        /// <param name="pokemonName">The pokemons name like "Bulbasaur"</param>
        /// <returns>dictionary where the key is UserID and value is how many of the pokemon they have</returns>
        IReadOnlyDictionary<uint, uint> PokeRank(string pokemonName);

    }
}
