using PokemonBox.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using PokemonBox.Interfaces;

/*
 * Last updated: 4/17/2023
 * Procedures for translating ItemsOwned data from SQL to front-end
 * 
 * TODO: Fix create ItemsOwned
 *       Implement remaining methods
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

        public PokeOwned CreateItemsOwned(string userName, string itemName)
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
                        var u = command.Parameters.Add("OutUserName", SqlDbType.NVarChar);
                        u.Direction = ParameterDirection.Output;
                        var i = command.Parameters.Add("OutItemName", SqlDbType.NVarChar);
                        i.Direction = ParameterDirection.Output;
                        connection.Open();
                        command.ExecuteNonQuery();
                        transaction.Complete();
                        var userName = (string)command.Parameters["OutUserName"].Value;
                        var itemName = (string)command.Parameters["OutItemName"].Value;
                        return new ItemsOwned(userName, itemName);
                    }
                }
            }
        }

        public void RemoveItemsOwned(string userName, string itemName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ItemsOwned> SelectAllPItemsOwnedByUser(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
