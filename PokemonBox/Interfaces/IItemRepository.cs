using System.Collections.Generic;
using PokemonBox.Models;

/*
 * Last updated: 4/17/2023
 * Interface for items
 * 
 * TODO: Review / update based on feedback
 *       Are we implementing remove item? (or remove item type for that matter)
 */

namespace PokemonBox.Interfaces
{
    public interface IItemRepository
    {
        IReadOnlyList<Item> SelectItem();
        Pokemon GetItem(string itemName);
        Pokemon FetchItem(uint itemID);
        Pokemon AddItem(string itemName);
    }
}
