using PokemonBox.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

/*
 * Last updated: 4/17/2023
 * Procedures(?) for translating User data from SQL to front-end
 * 
 * TODO: Double-check accuracy of the above description :p
 */

namespace PokemonBox
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public SqlUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User AddUser(string userName, string password, string firstName, string lastName, bool isAdmin)
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
                    using (var command = new SqlCommand("Pokebox.AddUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("FirstName", firstName);
                        command.Parameters.AddWithValue("LastName", lastName);
                        command.Parameters.AddWithValue("UserName", userName);
                        command.Parameters.AddWithValue("Password", password);
                        command.Parameters.AddWithValue("IsAdmin", isAdmin);

                        var p = command.Parameters.Add("UserId", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var userID = (uint)command.Parameters["UserId"].Value;

                        return new User(userID, userName, password, firstName, lastName, isAdmin);
                    }
                }
            }
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
