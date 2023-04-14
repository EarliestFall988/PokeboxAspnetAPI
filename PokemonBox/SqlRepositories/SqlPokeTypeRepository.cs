using PokemonBox.Models;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace PokemonBox.SqlRepositories
{
    public class SqlPokeTypeRepository : IPokeTypeRepository
    {
        private readonly string _connectionString;

        public SqlPokeTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PokeType AddPokeType(string pokemonTypeName, string pokemonName)
        {
            if (pokemonTypeName == null)
                throw new ArgumentNullException(nameof(pokemonTypeName));
            if (pokemonName == null)
                throw new ArgumentNullException(nameof(pokemonName));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("Pokebox.AddPokeType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("PokemonTypeName", pokemonTypeName);
                        command.Parameters.AddWithValue("PokemonName", pokemonName);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokemonTypeID = (uint)command.Parameters["PokemonTypeID"].Value;
                        var pokemonID = (uint)command.Parameters["PokemonID"].Value;

                        return new PokeType(pokemonTypeID, pokemonID);
                    }
                }
            }
        }

        public IReadOnlyList<PokeType> SelectPokeType()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokemon.SelectPokemonType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslatePokeTypes(reader);
                    }
                }
            }
        }

        private IReadOnlyList<PokeType> TranslatePokeTypes(SqlDataReader reader)
        {
            var pokemonTypes = new List<PokeType>();

            var pokemonTypeID = (uint)reader.GetOrdinal("PokemonTypeID");
            var pokemonID = (uint)reader.GetOrdinal("PokemonID");

            while (reader.Read())
            {
                pokemonTypes.Add(new PokeType(pokemonID, pokemonTypeID));
            }

            return pokemonTypes;
        }
    }
}
