using PokemonBox.SqlRepositories;

using System.Transactions;

namespace PokemonBox.Utils
{
    public static class DatabaseConnection
    {
        public static Dictionary<string, string> Sessions = new Dictionary<string, string>();

        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        public static SqlItemRepository ItemRepo = new SqlItemRepository(connectionString);
        public static SqlItemsOwnedRepository ItemsOwnedRepo = new SqlItemsOwnedRepository(connectionString);
        public static SqlItemTypeRepository ItemTypeRepo = new SqlItemTypeRepository(connectionString);
        public static SqlPokemonRepository PokemonRepo = new SqlPokemonRepository(connectionString);
        public static SqlPokemonTypeRepository PokemonTypeRepo = new SqlPokemonTypeRepository(connectionString);
        public static SqlPokeOwnedRepository PokeOwnedRepo = new SqlPokeOwnedRepository(connectionString);
        public static SqlPokeTypeRepository PokeTypeRepo = new SqlPokeTypeRepository(connectionString);
        public static SqlUserRepository UserRepo = new SqlUserRepository(connectionString);

    }
}
