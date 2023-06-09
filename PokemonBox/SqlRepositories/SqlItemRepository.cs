﻿using PokemonBox.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using PokemonBox.Interfaces;

/*
 * Last updated: 4/17/2023
 * Procedures for translating Item data from SQL to front-end
 * 
 * TODO:
 */

namespace PokemonBox.SqlRepositories
{
    public class SqlItemRepository : IItemRepository
    {
        private readonly string _connectionString;

        public SqlItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Item AddItem(string itemName, string description, string itemTypeName, string itemImageLink)
        {
            if (itemName == null) throw new ArgumentNullException(nameof(itemName));
            if (description == null) throw new ArgumentNullException(nameof(description));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("Pokebox.AddItem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("ItemName", itemName);
                        command.Parameters.AddWithValue("Description", description);
                        command.Parameters.AddWithValue("ItemTypeName", itemTypeName);
                        command.Parameters.AddWithValue("ItemImageLink", itemImageLink);

                        var iID = command.Parameters.Add("ItemID", SqlDbType.Int);
                        iID.Direction = ParameterDirection.Output;
                        var iTID = command.Parameters.Add("OutItemTypeID", SqlDbType.Int);
                        iTID.Direction = ParameterDirection.Output;
                        var date = command.Parameters.Add("DateAdded", SqlDbType.DateTimeOffset);
                        date.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var itemID = (int)command.Parameters["ItemID"].Value;
                        var itemTypeID = (int)command.Parameters["OutItemTypeID"].Value;
                        var dateAdded = (DateTimeOffset)command.Parameters["DateAdded"].Value;

                        return new Item((uint)itemID, (uint)itemTypeID, itemName, dateAdded, description, itemImageLink);
                    }
                }
            }
        }

        // TODO: triple-check / completely rework this
        public Item FetchItem(uint itemID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("User.FetchItem", connection))
                {
                    command.Parameters.Add(itemID);

                    connection.Open();

                    using (var reader = command.ExecuteReader()) 
                    {
                        var user = TranslateItem(reader);

                        if (user == null) throw new RecordNotFoundException(itemID.ToString());

                        return user;
                    }
                }
            }
        }

        public Item GetItem(string itemName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("User.GetItem", connection))
                {
                    command.Parameters.Add(itemName);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var user = TranslateItem(reader);

                        if (user == null) throw new RecordNotFoundException(itemName.ToString());

                        return user;
                    }
                }
            }
        }

        public IReadOnlyList<Item> SelectItem()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var items = TranslateItems(reader);

                        if (items == null) throw new RecordNotFoundException("No items found");

                        return items;
                    }
                }
            }

        }

        public uint SelectItemCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectItemCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var itemID = reader.GetOrdinal("ItemCount");
                        reader.Read();
                        return (uint)reader.GetInt32(itemID);
                    }
                }
            }
        }

        private Item TranslateItem(SqlDataReader reader)
        {
            var itemID = reader.GetOrdinal("ItemID");
            var itemTypeID = reader.GetOrdinal("ItemTypeID");
            var itemName = reader.GetOrdinal("ItemName");
            var dateAdded = reader.GetOrdinal("DateAdded");
            var description = reader.GetOrdinal("Description");
            var itemLink = reader.GetOrdinal("ItemImageLink");
            reader.Read();
            return new Item(
                (uint)reader.GetInt32(itemID),
                (uint)reader.GetInt32(itemTypeID),
                reader.GetString(itemName),
                reader.GetDateTimeOffset(dateAdded),
                reader.GetString(description),
                reader.GetString(itemLink)
            );
        }

        private IReadOnlyList<Item> TranslateItems(SqlDataReader reader)
        {
            var items = new List<Item>();

            var itemID = reader.GetOrdinal("ItemID");
            var itemTypeID = reader.GetOrdinal("ItemTypeID");
            var itemName = reader.GetOrdinal("ItemName");
            var dateAdded = reader.GetOrdinal("DateAdded");
            var description = reader.GetOrdinal("Description");
            var itemImageLink = reader.GetOrdinal("ItemImageLink");

            while(reader.Read())
            {
                var item = new Item(
                    (uint)reader.GetInt32(itemID),
                    (uint)reader.GetInt32(itemTypeID),
                    reader.GetString(itemName),
                    reader.GetDateTimeOffset(dateAdded),
                    reader.GetString(description),
                    reader.GetString(itemImageLink)
                );

                items.Add(item);
            }

            return items;
        }
    }
}
