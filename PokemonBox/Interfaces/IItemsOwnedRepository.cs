﻿using System.Collections.Generic;
using PokemonBox.Models;

/*
 * Last updated: 4/17/2023
 * Interface for items owned
 * 
 * TODO:
 */

namespace PokemonBox.Interfaces
{
    public interface IItemsOwnedRepository
    {
        ItemsOwned CreateItemsOwned(string userName, string itemName);

        void RemoveItemsOwned(string userName, string itemName);

        IReadOnlyList<ItemsOwned> SelectAllItemsOwnedByUser(string userName);
    }
}
