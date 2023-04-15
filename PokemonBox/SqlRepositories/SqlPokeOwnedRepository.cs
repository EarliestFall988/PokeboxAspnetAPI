using PokemonBox.Models;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace PokemonBox
{
    public class SqlPokeOwnedRepository : IPokeOwnedRepository
    {
        private readonly string _connectionString;

        public SqlPokeOwnedRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PokemonType AddPokemonType(string typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("Pokebox.AddPokemonType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("PokemonTypeName", typeName);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokemonTypeID = (uint)command.Parameters["PokemonTypeID"].Value;

                        return new PokemonType(pokemonTypeID, typeName);
                    }
                }
            }
        }

        public PokeOwned CreatePokeOwned(string userName, string pokemonName, string name, pokeGender gender, uint level)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));
            if (pokemonName == null)
                throw new ArgumentNullException(nameof(pokemonName));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("Pokebox.AddPokeOwned", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("Username", userName);
                        command.Parameters.AddWithValue("PokemonName", pokemonName);
                        command.Parameters.AddWithValue("Name", name);
                        command.Parameters.AddWithValue("Gender", gender);
                        command.Parameters.AddWithValue("Level", level);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokeOwnedID = (uint)command.Parameters["PokeOwnedID"].Value;
                        var userID = (uint)command.Parameters["UserID"].Value;
                        var pokemonID = (uint)command.Parameters["PokemonID"].Value;
                        var datePutInBox = (DateTimeOffset)command.Parameters["DatePutInBox"].Value;

                        return new PokeOwned(pokeOwnedID, userID, pokemonID, pokemonName, datePutInBox, gender, level);
                    }
                }
            }
        }

        public void RemovePokeOwned(uint userID, uint pokemonID, string pokeName)
        {
            throw new NotImplementedException();
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
