using PokemonBox.Models;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using System.Xml.Linq;

namespace PokemonBox
{
    public class SqlPokeOwnedRepository : IPokeOwnedRepository
    {
        private readonly string _connectionString;

        public SqlPokeOwnedRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PokeOwned CreatePokeOwned(string userName, string pokemonName, string nickName, pokeGender gender, uint level)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));
            if (pokemonName == null)
                throw new ArgumentNullException(nameof(pokemonName));
            if (nickName == null)
                throw new ArgumentNullException(nameof(nickName));
            if (pokemonName == null)
                throw new ArgumentNullException(nameof(pokemonName));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("Pokebox.AddPokemonOwned", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("Username", userName);
                        command.Parameters.AddWithValue("PokemonName", pokemonName);
                        command.Parameters.AddWithValue("Name", nickName);
                        command.Parameters.AddWithValue("Gender", gender);
                        command.Parameters.AddWithValue("Level", (int)level);

                        var p = command.Parameters.Add("OutPokeID", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;
                        var u = command.Parameters.Add("OutUserID", SqlDbType.Int);
                        u.Direction = ParameterDirection.Output;
                        var o = command.Parameters.Add("PokeOwnedID", SqlDbType.Int);
                        o.Direction = ParameterDirection.Output;
                        var d = command.Parameters.Add("DatePutInBox", SqlDbType.DateTimeOffset);
                        d.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokeOwnedID = (int)command.Parameters["PokeOwnedID"].Value;
                        var userID = (int)command.Parameters["OutUserID"].Value;
                        var pokemonID = (int)command.Parameters["OutPokeID"].Value;
                        var datePutInBox = (DateTimeOffset)command.Parameters["DatePutInBox"].Value;

                        return new PokeOwned((uint)pokeOwnedID, (uint)userID, (uint)pokemonID, nickName, datePutInBox, gender, level);
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
            var name = reader.GetOrdinal("Name");
            var datePutInBox = reader.GetOrdinal("DatePutInBox");
            var gender = reader.GetOrdinal("Gender");
            var level = reader.GetOrdinal("Level");

            while (reader.Read())
            {
                var oID = reader.GetInt32(pokeOwnedID);
                var uID = reader.GetInt32(userID);
                var pID = reader.GetInt32(pokemonID);
                var g = (pokeGender)reader.GetInt32(gender);
                var l = (uint)reader.GetInt32(level);
                var nickName = reader.GetString(name);
                var date = reader.GetDateTimeOffset(datePutInBox);
                pokeOwned.Add(new PokeOwned((uint)oID, (uint)uID, (uint)pID, nickName, date, g, l));
            }

            return pokeOwned;
        }

        private void RemovePokeOwned(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public PokeOwned SelectSinglePokeOwned(string userName, string pokemonName, string nickName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectSinglePokeOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("PokemonName", pokemonName);
                    command.Parameters.AddWithValue("Name", nickName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateSinglePokeOwned(reader);
                    }
                }
            }
        }

        private PokeOwned TranslateSinglePokeOwned(SqlDataReader reader)
        {

            var pokeOwnedID = reader.GetOrdinal("PokeOwnedID");
            var userID = reader.GetOrdinal("UserID");
            var pokemonID = reader.GetOrdinal("PokemonID");
            var name = reader.GetOrdinal("Name");
            var datePutInBox = reader.GetOrdinal("DatePutInBox");
            var gender = reader.GetOrdinal("Gender");
            var level = reader.GetOrdinal("Level");

            var oID = reader.GetInt32(pokeOwnedID);
            var uID = reader.GetInt32(userID);
            var pID = reader.GetInt32(pokemonID);
            var g = (pokeGender)reader.GetInt32(gender);
            var l = (uint)reader.GetInt32(level);
            var nickName = reader.GetString(name);
            var date = reader.GetDateTimeOffset(datePutInBox);
            return new PokeOwned((uint)oID, (uint)uID, (uint)pID, nickName, date, g, l);
        }
    }
}
