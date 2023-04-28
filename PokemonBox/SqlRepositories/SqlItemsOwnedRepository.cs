using PokemonBox.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using PokemonBox.Interfaces;
using System.ComponentModel.DataAnnotations;
using PokemonBox.Controllers;

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
                using (var command = new SqlCommand("Pokebox.RemoveItemOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("ItemName", itemName);

                    connection.Open();

                    var reader = command.ExecuteReader();
                }
            }
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

            reader.Read();

            var oID = (uint)reader.GetInt32(itemOwnedID);
            var uID = (uint)reader.GetInt32(userID);
            var iID = (uint)reader.GetInt32(itemID);
            var date = reader.GetDateTimeOffset(datePutInBox);

            return new ItemsOwned(oID, uID, iID, date);
        }

        private List<ItemOwnedPresentation> TranslateItemsOwnedOffset(SqlDataReader reader)
        {
            var itemsOwnedPres = new List<ItemOwnedPresentation>();

            var datePutInBox = reader.GetOrdinal("DatePutInBox");
            var itemTypeName = reader.GetOrdinal("ItemTypeName");
            var itemImageLink = reader.GetOrdinal("ItemImageLink");
            var itemName = reader.GetOrdinal("ItemName");
            var description = reader.GetOrdinal("Description");

            while (reader.Read())
            {
                var date = reader.GetDateTimeOffset(datePutInBox);
                var type = reader.GetString(itemTypeName);
                var image = reader.GetString(itemImageLink);
                var name = reader.GetString(itemName);
                var desc = reader.GetString(description);

                itemsOwnedPres.Add(new ItemOwnedPresentation(date, type, image, name, desc));
            }

            return itemsOwnedPres;
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
        
        public IReadOnlyList<TopItem> TopItem(uint year, uint month)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.TopItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Month", (int)month);
                    command.Parameters.AddWithValue("Year", (int)year);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateTopItem(reader);
                    }
                }
            }
        }

        private IReadOnlyList<TopItem> TranslateTopItem(SqlDataReader reader) 
        { 
            var dict = new List<TopItem>();

            var itemID = reader.GetOrdinal("ItemID");
            var itemCount = reader.GetOrdinal("ItemCount");
            var itemName = reader.GetOrdinal("ItemName");
            var itemImage = reader.GetOrdinal("ItemImageLink");
            uint count = 1;

            while (reader.Read())
            {
                var iID = (uint)reader.GetInt32(itemID);
                var iC = (uint)reader.GetInt32(itemCount);
                var iN = reader.GetString(itemName);
                var iI = reader.GetString(itemImage);

                var t = new TopItem(iID, iN, iC, iI, count);
                count++;
                dict.Add(t);
            }

            return dict;
        }

        public IReadOnlyList<ItemOwnedPresentation> SelectAllItemsOwnedByUserOffset(string userName, uint pageNum)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectAllItemOwnedOffset", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("Page", (int)pageNum);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateItemsOwnedOffset(reader);
                    }
                }
            }
        }

        public uint SelectAllItemsOwnedByUserOffsetPages(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectAllItemsOwnedByUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);

                    connection.Open();
                    IReadOnlyList<ItemsOwned> item;

                    using (var reader = command.ExecuteReader())
                    {
                        item = TranslateItemsOwned(reader);
                    }
                    double num = item.Count / 30.0;
                    return (uint)Math.Ceiling(num);

                }
            }
        }

        public int FetchItemOwned(string userName, uint itemOwnedID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.FetchSingleItemOwned", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Username", userName);
                    command.Parameters.AddWithValue("ItemOwnedID", (int)itemOwnedID);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateFetchSingleItemOwned(reader);
                    }
                }
            }
        }

        private int TranslateFetchSingleItemOwned(SqlDataReader reader)
        {
            var id = reader.GetOrdinal("ItemID");
            reader.Read();
            var itemID = reader.GetInt32(id);
            return itemID;
        }

    }
}
