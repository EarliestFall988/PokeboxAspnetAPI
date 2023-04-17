using System.Collections.Generic;
using PokemonBox.Models;

namespace PokemonBox
{
    public interface IUserRepository
    {
        IReadOnlyList<User> SelectUser();

        User AddUser(string userName, string password, string firstName, string lastName, bool isAdmin);
    }
}
