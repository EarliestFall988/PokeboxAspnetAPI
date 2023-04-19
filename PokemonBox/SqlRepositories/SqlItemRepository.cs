using PokemonBox.Models;
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

        public Item AddItem(string itemName, string description)
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

                        var iID = command.Parameters.Add("ItemID", SqlDbType.Int);
                        iID.Direction = ParameterDirection.Output;
                        var date = command.Parameters.Add("DateAdded", SqlDbType.DateTimeOffset);
                        date.Direction = ParameterDirection.Output;

                        connection.Open();

                        command.ExecuteNonQuery();

                        transaction.Complete();

                        var itemID = (int)command.Parameters["ItemID"].Value;
                        var itemTypeID = (int)command.Parameters["ItemTypeID"].Value;
                        var dateAdded = (DateTimeOffset)command.Parameters["DateAdded"].Value;

                        return new Item((uint)itemID, (uint)itemTypeID, itemName, dateAdded, description);
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
                using (var command = new SqlCommand("User.FetchItem", connection))
                {
                    command.Parameters.Add(itemName);

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
            throw new NotImplementedException();
        }

        private Item TranslateItem(SqlDataReader reader)
        {
            var itemID = reader.GetOrdinal("ItemID");
            var itemTypeID = reader.GetOrdinal("ItemTypeID");
            var itemName = reader.GetOrdinal("ItemName");
            var dateAdded = reader.GetOrdinal("DateAdded");
            var description = reader.GetOrdinal("Description");

            return new Item(
                (uint)reader.GetInt32(itemID),
                (uint)reader.GetInt32(itemTypeID),
                reader.GetString(itemName),
                reader.GetDateTimeOffset(dateAdded),
                reader.GetString(description)
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

            while(reader.Read())
            {
                var item = new Item(
                    (uint)reader.GetInt32(itemID),
                    (uint)reader.GetInt32(itemTypeID),
                    reader.GetString(itemName),
                    reader.GetDateTimeOffset(dateAdded),
                    reader.GetString(description)
                );

                items.Add(item);
            }

            return items;
        }
    }
}
