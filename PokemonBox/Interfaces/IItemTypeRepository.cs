using System.Collections.Generic;
using PokemonBox.Models;

/*
 * Last updated: 4/17/2023
 * Interface for item types
 * 
 * TODO: Review / update based on feedback
 */

namespace PokemonBox.Interfaces
{
    public interface IItemTypeRepository
    {
        IReadOnlyList<ItemType> SelectItemType();
        ItemType AddItemType(string itemTypeName, string itemName);
    }
}
