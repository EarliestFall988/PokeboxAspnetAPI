using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using PokemonBox.Models;

namespace PokemonBox
{
    public class SqlPokemonRepository : IPokemonRepository
    {
        private readonly string _connectionString;

        public SqlPokemonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SavePokemon(uint pokemonID, string pokemonName, uint pokedexNumber, string decription, DateTimeOffset dateAdded, bool isLegendary)
        {
            if (pokemonName == null)
                throw new ArgumentNullException(nameof(pokemonName));

            if(pokedexNumber == 0)
                throw new ArgumentNullException(nameof(pokedexNumber));

            if(decription == null) 
                throw new ArgumentNullException(nameof(decription));

            if (dateAdded.Year < 1960)
                throw new ArgumentOutOfRangeException(nameof(dateAdded));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {                                       //TODO: Rename to proper procedure
                    using (var command = new SqlCommand("Pokemon.SavePokemon", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("PokemonID", pokemonID);
                        command.Parameters.AddWithValue("PokemonName", pokemonName);
                        command.Parameters.AddWithValue("PokedexNumber", pokedexNumber);
                        command.Parameters.AddWithValue("Decription", decription);
                        command.Parameters.AddWithValue("DateAdded", dateAdded);
                        command.Parameters.AddWithValue("IsLegendary", isLegendary);

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();
                    }
                }
            }

        }

        public IReadOnlyList<Pokemon> RetrievePokemon(uint pokemonID)
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {                                       //TODO: Rename to proper procedure
                using (var command = new SqlCommand("Pokemon.RetrieveAddressesForPerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("PokemonID", pokemonID);

                    connection.Open();

                    using (var reader = command.ExecuteReader()) 
                    {
                        return TranslatePokemon(reader);
                    }
                }
            }
        }

        private IReadOnlyList<Pokemon> TranslatePokemon(SqlDataReader reader)
        {
            //my plural distinction of pokemon, pokeman
            var pokeman = new List<Pokemon>();

            var pokemonID = reader.GetOrdinal("PokemonID");
            var pokemonName = reader.GetOrdinal("PokemonName");
            var pokedexNumber = reader.GetOrdinal("PokedexNumber");
            var decription = reader.GetOrdinal("Decription");
            var dateAdded = reader.GetOrdinal("DateAdded");
            var isLegendary = reader.GetOrdinal("IsLegendary");

            while(reader.Read())
            {
                pokeman.Add(new Pokemon(
                    (uint)reader.GetInt32(pokemonID),
                    reader.GetString(pokemonName),
                    (uint)reader.GetInt32(pokedexNumber),
                    reader.GetString(decription),
                    reader.GetDateTimeOffset(dateAdded),
                    reader.GetBoolean(isLegendary)));                    
            }

            return pokeman;
        }

    }
}
