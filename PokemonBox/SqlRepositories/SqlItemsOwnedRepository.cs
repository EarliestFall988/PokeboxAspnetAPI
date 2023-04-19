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
                    using (var command = new SqlCommand("Pokebox.AddItemOwned", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("UserName", userName);
                        command.Parameters.AddWithValue("ItemName", itemName);

                        var p = command.Parameters.Add("ItemOwnedID", SqlDbType.Int);
                        p.Direction = ParameterDirection.Output;
                        var u = command.Parameters.Add("OutUserID", SqlDbType.Int);
                        u.Direction = ParameterDirection.Output;
                        var o = command.Parameters.Add("OutItemID", SqlDbType.Int);
                        o.Direction = ParameterDirection.Output;
                        var d = command.Parameters.Add("DatePutInBox", SqlDbType.DateTimeOffset);
                        d.Direction = ParameterDirection.Output;

                        connection.Open();
                        command.ExecuteNonQuery();
                        transaction.Complete();
                        var itemsOwnedID = (int)command.Parameters["ItemOwnedID"].Value;
                        var userID = (int)command.Parameters["OutUserID"].Value;
                        var itemID = (int)command.Parameters["OutItemID"].Value;
                        var datePutInBox = (DateTimeOffset)command.Parameters["DatePutInBox"].Value;
                        return new ItemsOwned((uint)itemsOwnedID, (uint)userID, (uint)itemID, datePutInBox);
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

        private ItemsOwned TranslateItemOwned(SqlDataReader reader)
        {
            var itemsOwned = new List<ItemsOwned>();

            var itemOwnedID = reader.GetOrdinal("ItemOwnedID");
            var userID = reader.GetOrdinal("UserID");
            var itemID = reader.GetOrdinal("ItemID");
            var datePutInBox = reader.GetOrdinal("DatePutInBox");

            var oID = (uint)reader.GetInt32(itemOwnedID);
            var uID = (uint)reader.GetInt32(userID);
            var iID = (uint)reader.GetInt32(itemID);
            var date = reader.GetDateTimeOffset(datePutInBox);

            return new ItemsOwned(oID, uID, iID, date);
        }

        public IReadOnlyList<ItemsOwned> SelectAllItemsOwned()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectItemOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateItemsOwned(reader);
                    }
                }
            }
        }

        public ItemsOwned SelectSingleItemOwned(string userName, string itemName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectSingleItemOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("ItemName", itemName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateItemOwned(reader);
                    }
                }
            }
        }

        public IReadOnlyDictionary<Item, uint> TopItem(DateTime year, DateTime month)
        {
            throw new NotImplementedException();
        }
    }
}
