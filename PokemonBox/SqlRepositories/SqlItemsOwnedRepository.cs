using PokemonBox.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using PokemonBox.Interfaces;
using System.ComponentModel.DataAnnotations;

/*
 * Last updated: 4/17/2023
 * Procedures for translating ItemsOwned data from SQL to front-end
 * 
 * TODO: Review & change based on feedback
 *       Implement private void RemovePokeOwned(SqlDataReader reader)
 */

namespace PokemonBox.SqlRepositories
{
    public class SqlItemsOwnedRepository : IItemsOwnedRepository
    {
        private readonly string _connectionString;

        public SqlItemsOwnedRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ItemsOwned CreateItemsOwned(string userName, string itemName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));
            if (itemName == null)
                throw new ArgumentNullException(nameof(itemName));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString)) 
                {
                    using (var command = new SqlCommand("Pokebox.CreateItemsOwned", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("UserName", userName);
                        command.Parameters.AddWithValue("ItemName", itemName);

                        connection.Open();
                        command.ExecuteNonQuery();
                        transaction.Complete();
                        var itemsOwnedID = (uint)command.Parameters["ItemsOwnedID"].Value;
                        var userID = (uint)command.Parameters["UserID"].Value;
                        var itemID = (uint)command.Parameters["ItemID"].Value;
                        var datePutInBox = (DateTimeOffset)command.Parameters["DatePutInBox"].Value;
                        return new ItemsOwned(itemsOwnedID, userID, itemID, datePutInBox);
                    }
                }
            }
        }

        public void RemoveItemsOwned(string userName, string itemName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.RemoveItemsOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("ItemName", itemName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        RemoveItemsOwned(reader);
                    }
                }
            }
        }

        private void RemoveItemsOwned(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ItemsOwned> SelectAllItemsOwnedByUser(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectAllItemsOwnedByUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateItemsOwned(reader);
                    }
                }
            }
        }

        private IReadOnlyList<ItemsOwned> TranslateItemsOwned(SqlDataReader reader)
        {
            var itemsOwned = new List<ItemsOwned>();

            var itemOwnedID = reader.GetOrdinal("ItemOwnedID");
            var userID = reader.GetOrdinal("UserID");
            var itemID = reader.GetOrdinal("ItemID");
            var datePutInBox = reader.GetOrdinal("DatePutInBox");

            while(reader.Read())
            {
                var oID = (uint)reader.GetInt32(itemOwnedID);
                var uID = (uint)reader.GetInt32(userID);
                var iID = (uint)reader.GetInt32(itemID);
                var date = reader.GetDateTimeOffset(datePutInBox);

                itemsOwned.Add(new ItemsOwned(oID, uID, iID, date));
            }

            return itemsOwned;
        }
    }
}
