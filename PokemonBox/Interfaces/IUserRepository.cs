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
        IReadOnlyList<User> SelectUser();

        User AddUser(string userName, string password, string firstName, string lastName, bool isAdmin);
    }
}
