﻿using PokemonBox.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace PokemonBox
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public SqlUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //TODO see if needed
        //public IReadOnlyList<Pokemon> RetrieveUserPokemon(uint userID)
        //{
        //    using(var connection = new SqlConnection(_connectionString))
        //    {
        //        using( var command = new SqlCommand("User.RetrieveUsersPokemon", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //
        //            command.Parameters.AddWithValue("UserID", userID);
        //
        //            connection.Open();
        //
        //            using( var reader = command.ExecuteReader())
        //            {
        //                return TranslatePokemon(reader);
        //            }
        //        }
        //    }
        //}

        public User CreateUser(uint itemsOwnedID, uint pokeOwnedID, string userName, string password, string firstName, string lastName, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(userName));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(password));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("User.CreateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("FirstName", firstName);
                        command.Parameters.AddWithValue("LastName", lastName);
                        command.Parameters.AddWithValue("UserName", userName);
                        command.Parameters.AddWithValue("Password", password);
                        command.Parameters.AddWithValue("ItemsOwnedID", itemsOwnedID);
                        command.Parameters.AddWithValue("PokeOwnedID", pokeOwnedID);
                        command.Parameters.AddWithValue("IsAdmin", isAdmin);

                        var p = command.Parameters.Add("UserId", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var userID = (uint)command.Parameters["UserId"].Value;

                        return new User(userID, itemsOwnedID, pokeOwnedID, userName, password, firstName, lastName, isAdmin);
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

            while (reader.Read())
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

        private IReadOnlyList<User> TranslateUsers(SqlDataReader reader)
        {
            //my plural distinction of pokemon, pokeman
            var users = new List<User>();

            var userID = reader.GetOrdinal("UserID");
            var itemsOwnedID = reader.GetOrdinal("ItemsOwnedID");
            var pokeOwnedID = reader.GetOrdinal("PokeOwnedID");
            var userName = reader.GetOrdinal("UserName");
            var password = reader.GetOrdinal("Password");
            var firstName = reader.GetOrdinal("FirstName");
            var lastName = reader.GetOrdinal("LastName");
            var isAdmin = reader.GetOrdinal("IsAdmin");

            while (reader.Read())
            {
                users.Add(new User(
                    (uint)reader.GetInt32(userID),
                    (uint)reader.GetInt32(itemsOwnedID),
                    (uint)reader.GetInt32(pokeOwnedID),
                    reader.GetString(userName),
                    reader.GetString(password),
                    reader.GetString(firstName),
                    reader.GetString(lastName),
                    reader.GetBoolean(isAdmin)));
            }

            return users;
        }

        public User FetchUser(uint userID)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = new SqlCommand("User.FetchUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("UserID", userID);

                    connection.Open();

                    using(var reader = command.ExecuteReader())
                    {
                        var user = TranslateUser(reader);   

                        if(user == null)
                        {
                            throw new RecordNotFoundException(userID.ToString());
                        }

                        return user;
                    }
                }
            }
        }

        public User GetUser(string UserName)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = new SqlCommand("User.GetUserByUserName"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", UserName);

                    connection.Open();

                    using(var reader = command.ExecuteReader())
                    {
                        return TranslateUser(reader);
                    }
                }
            }
        }

        private User TranslateUser(SqlDataReader reader)
        {
            var userID = reader.GetOrdinal("UserID");
            var itemsOwnedID = reader.GetOrdinal("ItemsOwnedID");
            var pokeOwnedID = reader.GetOrdinal("PokeOwnedID");
            var userName = reader.GetOrdinal("UserName");
            var password = reader.GetOrdinal("Password");
            var firstName = reader.GetOrdinal("FirstName");
            var lastName = reader.GetOrdinal("LastName");
            var isAdmin = reader.GetOrdinal("IsAdmin");

            if(!reader.Read())
            {
                return null;
            }

            return new User(
                    (uint)reader.GetInt32(userID),
                    (uint)reader.GetInt32(itemsOwnedID),
                    (uint)reader.GetInt32(pokeOwnedID),
                    reader.GetString(userName),
                    reader.GetString(password),
                    reader.GetString(firstName),
                    reader.GetString(lastName),
                    reader.GetBoolean(isAdmin));
        }

        public IReadOnlyList<User> RetrieveUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("User.RetrieveUsersPokemon", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateUsers(reader);
                    }
                }
            }
        }
    }
}