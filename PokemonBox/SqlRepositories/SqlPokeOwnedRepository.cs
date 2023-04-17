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
                    using (var command = new SqlCommand("Pokebox.AddPokemonOwned", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("Username", userName);
                        command.Parameters.AddWithValue("PokemonName", pokemonName);
                        command.Parameters.AddWithValue("Name", name);
                        command.Parameters.AddWithValue("Gender", gender);
                        command.Parameters.AddWithValue("Level", (int)level);

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

        public void RemovePokeOwned(string userName, string pokemonName, string pokeName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.RemovePokeOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("PokemonName", pokemonName);
                    command.Parameters.AddWithValue("Name", pokeName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        RemovePokeOwned(reader);
                    }

                }
            }
        }

        public IReadOnlyList<PokeOwned> SelectAllPokemonOwnedByUser(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectAllPokemonOwnedByUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslatePokeOwned(reader);
                    }
                }
            }
        }

        private IReadOnlyList<PokeOwned> TranslatePokeOwned(SqlDataReader reader)
        {
            var pokeOwned = new List<PokeOwned>();

            var pokeOwnedID = reader.GetOrdinal("PokeOwnedID");
            var userID = reader.GetOrdinal("UserID");
            var pokemonID = reader.GetOrdinal("PokemonID");
            var pokeName = reader.GetOrdinal("PokeName");
            var datePutInBox = reader.GetOrdinal("DatePutInBox");
            var gender = reader.GetOrdinal("Gender");
            var level = reader.GetOrdinal("Level");

            while (reader.Read())
            {
                var oID = (uint)reader.GetInt32(pokeOwnedID);
                var uID = (uint)reader.GetInt32(userID);
                var pID = (uint)reader.GetInt32(pokemonID);
                var g = (pokeGender)reader.GetInt32(gender);
                var l = (uint)reader.GetInt32(level);
                var name = reader.GetString(pokeName);
                var date = reader.GetDateTimeOffset(datePutInBox);
                pokeOwned.Add(new PokeOwned(oID, uID, pID, name, date, g, l));
            }

            return pokeOwned;
        }

        private void RemovePokeOwned(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
