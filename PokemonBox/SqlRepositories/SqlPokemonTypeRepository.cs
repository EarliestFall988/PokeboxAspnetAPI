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
                    using (var command = new SqlCommand("Pokebox.AddPokemonType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("TypeName", TypeName);

                        var p = command.Parameters.Add("PokemonTypeId", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokemonTypeID = (int)command.Parameters["PokemonTypeID"].Value;

                        return new PokemonType((uint)pokemonTypeID, TypeName);
                    }
                }
            }
        }

        public IReadOnlyList<PokemonType> SelectPokemonTypes()
        {
            using (var connection = new SqlConnection(_connectionString))
            {                                       
                using (var command = new SqlCommand("Pokebox.SelectPokemonType", connection))
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

            var pokemonTypeID = reader.GetOrdinal("PokemonTypeID");
            var pokemonTypeName = reader.GetOrdinal("PokemonTypeName");

            while (reader.Read())
            {
                var id = (uint)reader.GetInt32(pokemonTypeID);
                var name = reader.GetString(pokemonTypeName);
                pokemonTypes.Add(new PokemonType(id, name));
            }

            return pokemonTypes;
        }
    }
}
