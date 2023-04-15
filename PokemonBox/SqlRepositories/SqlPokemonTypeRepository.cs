using PokemonBox.Models;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace PokemonBox.SqlRepositories
{
    public class SqlPokemonTypeRepository : IPokemonTypeRepository
    {
        private readonly string _connectionString;

        public SqlPokemonTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PokemonType AddPokemonType(string TypeName)
        {
            if (TypeName == null)
                throw new ArgumentNullException(nameof(TypeName));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("Pokebox.AddPokemon", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("PokemonTypeName", TypeName);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokemonTypeID = (uint)command.Parameters["PokemonTypeID"].Value;

                        return new PokemonType(pokemonTypeID, TypeName);
                    }
                }
            }
        }

        public IReadOnlyList<PokemonType> SelectPokemonTypes()
        {
            using (var connection = new SqlConnection(_connectionString))
            {                                       
                using (var command = new SqlCommand("Pokemon.SelectPokemonType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslatePokemonTypes(reader);
                    }
                }
            }
        }

        private IReadOnlyList<PokemonType> TranslatePokemonTypes(SqlDataReader reader)
        {
            var pokemonTypes = new List<PokemonType>();

            var pokemonTypeID = (uint)reader.GetOrdinal("PokemonID");
            var pokemonTypeName = reader.GetString("PokemonName");

            while (reader.Read())
            {
                pokemonTypes.Add(new PokemonType(pokemonTypeID, pokemonTypeName));
            }

            return pokemonTypes;
        }
    }
}
