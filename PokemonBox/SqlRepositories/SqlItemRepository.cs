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

        public Item AddItem(string itemName, string description, string itemTypeName)
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

                        return new Item((uint)itemID, (uint)itemTypeID, itemName, dateAdded, description);
                    }
                }
            }
        }

        //public Item FetchItem(uint itemID)
        //{
        //    throw new NotImplementedException();
        //}
        //
        //public Item GetItem(string itemName)
        //{
        //    throw new NotImplementedException();
        //}
        //
        public IReadOnlyList<Item> SelectItem()
        {
            throw new NotImplementedException();
        }
    }
}
