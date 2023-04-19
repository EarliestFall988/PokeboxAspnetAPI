using PokemonBox.SqlRepositories;

using System.Transactions;

namespace PokemonBox.Utils
{
    public static class DatabaseConnection
    {
        public static Dictionary<string, string> Sessions = new Dictionary<string, string>();

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
