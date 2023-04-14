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
                    using (var command = new SqlCommand("Pokebox.AddPokemon", connection))
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

                        //var pokemonTypeID = (uint)command.Parameters["PokemonTypeID"].Value;
                        //var pokemonTypeID = (uint)command.Parameters["PokemonTypeID"].Value;
                        //var pokemonTypeID = (uint)command.Parameters["PokemonTypeID"].Value;
                        //
                        //return new PokeOwned();
                        return null;
                    }
                }
            }
        }

        public void RemovePokeOwned(uint userID, uint pokemonID, string pokeName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<PokemonType> SelectPokemonType()
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
