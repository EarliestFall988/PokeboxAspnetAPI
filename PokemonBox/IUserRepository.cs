using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IUserRepository
    {
        IReadOnlyList<User> RetrieveUsers();

        IReadOnlyList<Pokemon> RetrieveUserPokemon(uint userID);

        /// <summary>
        /// Gets the user with the given userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        User FetchUser(uint userID);

        /// <summary>
        /// Gets the user with the given username
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        User GetUser(string UserName);

        User CreateUser(uint itemsOwnedID, uint pokeOwnedID, string userName, string password, string firstName, string lastName, bool isAdmin);

    }
}
