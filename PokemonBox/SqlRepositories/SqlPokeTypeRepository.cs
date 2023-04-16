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

                        var p = command.Parameters.Add("OutPokeID", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;
                        var t = command.Parameters.Add("OutTypeID", SqlDbType.Int);
                        t.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokemonTypeID = (int)command.Parameters["OutTypeID"].Value;
                        var pokemonID = (int)command.Parameters["OutPokeID"].Value;

                        return new PokeType((uint)pokemonID, (uint)pokemonTypeID);
                    }
                }
            }
        }

        public IReadOnlyList<PokeType> SelectPokeType()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectPokeType", connection))
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

            var pokemonTypeID = reader.GetOrdinal("PokemonTypeID");
            var pokemonID = reader.GetOrdinal("PokemonID");

            while (reader.Read())
            {
                var PokeId = (uint)reader.GetInt32(pokemonTypeID);
                var TypeId = (uint)reader.GetInt32(pokemonID);
                pokemonTypes.Add(new PokeType(PokeId, TypeId));
            }

            return pokemonTypes;
        }
    }
}
