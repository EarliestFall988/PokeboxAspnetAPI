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

        public Pokemon AddPokemon(string pokemonName, uint pokedexNumber, string imageLink, bool isLegendary)
        {
            if (pokemonName == null)
                throw new ArgumentNullException(nameof(pokemonName));

            if(pokedexNumber == 0)
                throw new ArgumentNullException(nameof(pokedexNumber));

            if(imageLink == null) 
                throw new ArgumentNullException(nameof(imageLink));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {                                       
                    using (var command = new SqlCommand("Pokebox.AddPokemon", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("PokemonName", pokemonName);
                        command.Parameters.AddWithValue("PokedexNumber", (int)pokedexNumber);
                        command.Parameters.AddWithValue("ImageLink", imageLink);
                        command.Parameters.AddWithValue("IsLegendary", isLegendary);

                        var p = command.Parameters.Add("PokemonID", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;
                        var t = command.Parameters.Add("DateAdded", SqlDbType.DateTimeOffset);
                        t.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var pokemonID = (int)command.Parameters["PokemonID"].Value;
                        var dateAdded = (DateTimeOffset)command.Parameters["DateAdded"].Value;

                        return new Pokemon((uint)pokemonID, pokemonName, pokedexNumber, imageLink, dateAdded, isLegendary);
                    }
                }
            }

        }

        //public Pokemon FetchPokemon(uint pokemonID)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        using (var command = new SqlCommand("User.FetchPokemon", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //
        //            command.Parameters.AddWithValue("PokemonID", pokemonID);
        //
        //            connection.Open();
        //
        //            using (var reader = command.ExecuteReader())
        //            {
        //                var user = TranslatePokemon(reader);
        //
        //                if (user == null)
        //                {
        //                    throw new RecordNotFoundException(pokemonID.ToString());
        //                }
        //
        //                return user;
        //            }
        //        }
        //    }
        //}

        //public Pokemon GetPokemon(string pokemonName)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        using (var command = new SqlCommand("User.GetUserByUserName"))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //
        //            command.Parameters.AddWithValue("PokemonName", pokemonName);
        //
        //            connection.Open();
        //
        //            using (var reader = command.ExecuteReader())
        //            {
        //                return TranslatePokemon(reader);
        //            }
        //        }
        //    }
        //}

        public IReadOnlyList<Pokemon> SelectPokemon()
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {                                       
                using (var command = new SqlCommand("Pokebox.SelectPokemon", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader()) 
                    {
                        return TranslatePokeman(reader);
                    }
                }
            }
        }

        private Pokemon TranslatePokemon(SqlDataReader reader)
        {
            var pokemonID = reader.GetOrdinal("PokemonID");
            var pokemonName = reader.GetOrdinal("PokemonName");
            var pokedexNumber = reader.GetOrdinal("PokedexNumber");
            var decription = reader.GetOrdinal("Decription");
            var dateAdded = reader.GetOrdinal("DateAdded");
            var isLegendary = reader.GetOrdinal("IsLegendary");

            return new Pokemon(
                    (uint)reader.GetInt32(pokemonID),
                    reader.GetString(pokemonName),
                    (uint)reader.GetInt32(pokedexNumber),
                    reader.GetString(decription),
                    reader.GetDateTimeOffset(dateAdded),
                    reader.GetBoolean(isLegendary));                    

        }

        private IReadOnlyList<Pokemon> TranslatePokeman(SqlDataReader reader)
        {
            //my plural distinction of pokemon, pokeman
            var pokeman = new List<Pokemon>();

            var pokemonID = reader.GetOrdinal("PokemonID");
            var pokemonName = reader.GetOrdinal("PokemonName");
            var pokedexNumber = reader.GetOrdinal("PokedexNumber");
            var decription = reader.GetOrdinal("ImageLink");
            var dateAdded = reader.GetOrdinal("DateAdded");
            var isLegendary = reader.GetOrdinal("IsLegendary");

            while (reader.Read())
            {
                bool leg;
                if(reader.GetInt32(isLegendary) == 1)
                {
                    leg = true;
                }
                else
                {
                    leg = false;
                }
                pokeman.Add(new Pokemon(
                    (uint)reader.GetInt32(pokemonID),
                    reader.GetString(pokemonName),
                    (uint)reader.GetInt32(pokedexNumber),
                    reader.GetString(decription),
                    reader.GetDateTimeOffset(dateAdded),
                    leg));
            }

            return pokeman;
        }

        public IReadOnlyDictionary<Pokemon, PokeType> PokeTypeCount()
        {
            throw new NotImplementedException();
        }
    }
}
