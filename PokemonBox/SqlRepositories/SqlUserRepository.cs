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

                        var userID = (int)command.Parameters["UserId"].Value;

                        return new User((uint)userID, userName, password, firstName, lastName, isAdmin);
                    }
                }
            }
        }

        private IReadOnlyList<User> TranslateUsers(SqlDataReader reader)
        {
            var users = new List<User>();

            var userID = reader.GetOrdinal("UserID");
            var userName = reader.GetOrdinal("UserName");
            var password = reader.GetOrdinal("Password");
            var firstName = reader.GetOrdinal("FirstName");
            var lastName = reader.GetOrdinal("LastName");
            var isAdmin = reader.GetOrdinal("IsAdmin");

            while (reader.Read())
            {
                bool admin;
                if(reader.GetInt32(isAdmin) == 1)
                {
                    admin = true;
                }
                else
                {
                    admin = false;
                }
                users.Add(new User(
                    (uint)reader.GetInt32(userID),
                    reader.GetString(userName),
                    reader.GetString(password),
                    reader.GetString(firstName),
                    reader.GetString(lastName),
                    admin));
            }

            return users;
        }

        private User TranslateUser(SqlDataReader reader)
        {
            var userID = reader.GetOrdinal("UserID");
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

        public IReadOnlyList<User> SelectUser()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectUser", connection))
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

        public User SelectSingleUser(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectSingleUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("UserName", userName);

                    var id = command.Parameters.Add("UserId", SqlDbType.Int);
                    id.Direction = ParameterDirection.Output;
                    var fn = command.Parameters.Add("FirstName", SqlDbType.NVarChar);
                    fn.Direction = ParameterDirection.Output;
                    var ln = command.Parameters.AddWithValue("LastName", SqlDbType.NVarChar);
                    ln.Direction = ParameterDirection.Output;
                    var p = command.Parameters.AddWithValue("Password", SqlDbType.NVarChar);
                    p.Direction = ParameterDirection.Output;
                    var a = command.Parameters.AddWithValue("IsAdmin", SqlDbType.Int);
                    a.Direction = ParameterDirection.Output;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateUser(reader);
                    }
                }
            }
        }
    }
}
