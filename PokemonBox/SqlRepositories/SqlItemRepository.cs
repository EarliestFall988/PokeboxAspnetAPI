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
        public Pokemon AddItem(string itemName)
        {
            throw new NotImplementedException();
        }

        public Pokemon FetchItem(uint itemID)
        {
            throw new NotImplementedException();
        }

        public Pokemon GetItem(string itemName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Item> SelectItem()
        {
            throw new NotImplementedException();
        }
    }
}
