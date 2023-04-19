﻿using PokemonBox.SqlRepositories;

using System.Transactions;

namespace PokemonBox.Utils
{
    public static class SessionStorage // this is technically a singleton, I know it's not good architecture....
    {
        public static Dictionary<string, string> Sessions = new Dictionary<string, string>();
    }

    public static class Database
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private static SqlItemTypeRepository ItemTypeRepo;
        private static TransactionScope transaction;

        public static SqlUserRepository UserRepo = new SqlUserRepository(connectionString);

        public static string GetUser(string sessionID)
        {
            return "";
        }
    }
}
