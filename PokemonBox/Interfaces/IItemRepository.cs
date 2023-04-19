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
        Item GetItem(string itemName);
        Item FetchItem(uint itemID);
        Item AddItem(string itemName, string description, string itemTypeName);
    }
}
