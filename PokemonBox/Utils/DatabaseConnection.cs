using PokemonBox.SqlRepositories;

using System.Transactions;

namespace PokemonBox.Utils
{
    public static class DatabaseConnection
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
