using PokemonBox.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using PokemonBox.Interfaces;

/*
 * Last updated: 4/17/2023
 * Procedures for translating ItemType data from SQL to front-end
 * 
 * TODO: Review and update based on feedback
 */

namespace PokemonBox.SqlRepositories
{
    public class SqlItemTypeRepository : IItemTypeRepository
    {
        private readonly string _connectionString;

        public SqlItemTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ItemType AddItemType(string itemTypeName)
        {
            if (itemTypeName == null)
                throw new ArgumentNullException(nameof(itemTypeName));

            using (var transaction = new TransactionScope())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("Pokebox.AddItemType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("TypeName", itemTypeName);
                        var t = command.Parameters.Add("OutItemTypeID", SqlDbType.Int);
                        t.Direction = ParameterDirection.Output;
                        connection.Open();
                        command.ExecuteNonQuery();
                        transaction.Complete();
                        var itemTypeID = (int)command.Parameters["OutItemTypeID"].Value;
                        return new ItemType((uint)itemTypeID, itemTypeName);
                    }
                }
            }
        }

        public IReadOnlyList<ItemType> SelectItemType()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Pokebox.SelectItemType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return TranslateItemTypes(reader);
                    }
                }
            }
        }

        private IReadOnlyList<ItemType> TranslateItemTypes(SqlDataReader reader)
        {
            var itemTypes = new List<ItemType>();

            var itemTypeID = reader.GetOrdinal("ItemTypeID");
            var itemTypeName = reader.GetOrdinal("ItemTypeName");

            while (reader.Read())
            {
                var typeId = (uint)reader.GetInt32(itemTypeID);
                var typeName = reader.GetString(itemTypeName);
                itemTypes.Add(new ItemType(typeId, typeName));
            }

            return itemTypes;
        }
    }
}
