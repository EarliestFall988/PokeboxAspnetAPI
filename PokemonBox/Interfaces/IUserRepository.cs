using System.Collections.Generic;
using PokemonBox.Models;

/*
 * Last updated: 4/17/2023
 * Interface for user
 * 
 * TODO: Figure out and act on old TODOs
 */

namespace PokemonBox
{
    public interface IUserRepository
    {
        IReadOnlyList<User> RetrieveUsers();

        //TODO See if needed
        //IReadOnlyList<Pokemon> RetrieveUserPokemon(uint userID);

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

        User AddUser(string userName, string password, string firstName, string lastName, bool isAdmin);

        //TODO Add procedure that gets all pokemon owned by user
        //TODO Add procedure that gets all items owned by user

    }
}
