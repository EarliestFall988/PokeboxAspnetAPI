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

                    var reader = command.ExecuteReader();
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
                var gCheck = reader.GetString(gender);
                var l = (uint)reader.GetInt32(level);
                var nickName = reader.GetString(name);
                var date = reader.GetDateTimeOffset(datePutInBox);
                pokeGender g;

                if (gCheck.Equals("F"))
                {
                    g = pokeGender.female;
                }
                else if (gCheck.Equals("M"))
                {
                    g = pokeGender.male;
                }
                else
                {
                    g = pokeGender.unknown;
                }

                bool same = false;
                var newPoke = new PokeOwned((uint)oID, (uint)uID, (uint)pID, nickName, date, g, l);
                foreach (var p in pokeOwned)
                {
                    same = arePokeOwnedSame(p, newPoke);
                }
                if (!same)
                {
                    pokeOwned.Add(new PokeOwned((uint)oID, (uint)uID, (uint)pID, nickName, date, g, l));
                }

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

            reader.Read();
            var oID = reader.GetInt32(pokeOwnedID);
            var uID = reader.GetInt32(userID);
            var pID = reader.GetInt32(pokemonID);
            var gCheck = reader.GetString(gender);

            pokeGender g;

            if (gCheck.Equals("F"))
            {
                g = pokeGender.female;
            }
            else if (gCheck.Equals("M"))
            {
                g = pokeGender.male;
            }
            else
            {
                g = pokeGender.unknown;
            }

            var l = (uint)reader.GetInt32(level);
            var nickName = reader.GetString(name);
            var date = reader.GetDateTimeOffset(datePutInBox);
            return new PokeOwned((uint)oID, (uint)uID, (uint)pID, nickName, date, g, l);
        }

        public IReadOnlyList<PokeOwned> SelectAllPokemonOwned()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectPokeOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslatePokeOwned(reader);
                    }
                }
            }
        }

        private bool arePokeOwnedSame(PokeOwned a, PokeOwned b)
        {
            if (a.PokeOwnedID == b.PokeOwnedID)
            {
                return true;
            }
            else if (a.NickName == b.NickName && a.PokemonID == b.PokemonID)
            {
                return true;
            }
            return false;
        }

        public IReadOnlyDictionary<uint, decimal> AverageLevel()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.AverageLevel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return translateAverageLevel(reader);
                    }

                }
            }
        }

        private IReadOnlyDictionary<uint, decimal> translateAverageLevel(SqlDataReader reader)
        {
            var dic = new Dictionary<uint, decimal>();

            var u = reader.GetOrdinal("UserID");
            var av = reader.GetOrdinal("AveragePokeLevel");

            while (reader.Read())
            {
                var userID = reader.GetInt32(u);
                var average = reader.GetDecimal(av);

                dic.Add((uint)userID, average);
            }

            return dic;
        }

        public IReadOnlyDictionary<uint, uint> PokeRank(string pokemonName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.PokeRank", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("PokemonName", pokemonName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return translatePokeRank(reader);
                    }

                }
            }
        }

        private IReadOnlyDictionary<uint, uint> translatePokeRank(SqlDataReader reader)
        {
            var dic = new Dictionary<uint, uint>();

            var u = reader.GetOrdinal("UserID");
            var p = reader.GetOrdinal("PokemonCount");

            while (reader.Read())
            {
                var userID = reader.GetInt32(u);
                var pokemonCount = reader.GetInt32(p);

                dic.Add((uint)userID, (uint)pokemonCount);
            }

            return dic;
        }

        public IReadOnlyDictionary<string, uint> PokeTypeCount(DateTimeOffset start, DateTimeOffset end)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.PokeTypeCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("StartDate", start);
                    command.Parameters.AddWithValue("FinalDate", end);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return translatePokeTypeCount(reader);
                    }

                }
            }
        }

        private IReadOnlyDictionary<string, uint> translatePokeTypeCount(SqlDataReader reader)
        {
            var dic = new Dictionary<string, uint>();

            var pID = reader.GetOrdinal("PokemonTypeName");
            var p = reader.GetOrdinal("PokeTypeCount");

            while (reader.Read())
            {
                var pokeTypeID = reader.GetString(pID);
                var pokemonTypeCount = reader.GetInt32(p);

                dic.Add((string)pokeTypeID, (uint)pokemonTypeCount);
            }

            return dic;
        }

        public IReadOnlyList<PokeOwned> SelectAllPokemonOwnedByUserPages(string userName, uint pageNum)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectAllPokeOwnedOffset", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("Page", pageNum);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslatePokeOwned(reader);
                    }
                }
            }
        }

        public uint SelectAllPokemonOwnedByUserNumberPages(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectAllPokemonOwnedByUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);

                    connection.Open();
                    IReadOnlyList<PokeOwned> pokemon;

                    using (var reader = command.ExecuteReader())
                    {
                        pokemon = TranslatePokeOwned(reader);
                    }
                    double num = pokemon.Count / 30.0;

                    return (uint)Math.Ceiling(num);
                }
            }
        }
    }
}
