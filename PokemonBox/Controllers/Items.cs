using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using PokemonBox.Models;
using PokemonBox.SqlRepositories;
using PokemonBox.Utils;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokemonBox.Controllers
{
    [EnableCors("*")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Items : ControllerBase
    {

        /*********************************
        * 
        * OwnedItems Methods
        * 
        * ******************************/
        [HttpGet("SelectAllItemsOwned")]
        public string SelectAllItemsOwned() 
        {
            IReadOnlyList<ItemsOwned> items = DatabaseConnection.ItemsOwnedRepo.SelectAllItemsOwned();
            return JsonSerializer.Serialize(items);
        }

        [HttpGet("SelectAllItemsOwnedByUser")]
        public string SelectAllItemsOwnedByUser([FromHeader] string SessionId, [FromQuery] string username)
        {
            IReadOnlyList<ItemsOwned> items = DatabaseConnection.ItemsOwnedRepo.SelectAllItemsOwnedByUser(username);
            return JsonSerializer.Serialize(items);
        }

        [HttpPost("CreateItemsOwned")]
        public void CreateItemsOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string itemName)
        {
            ItemsOwned item = DatabaseConnection.ItemsOwnedRepo.CreateItemsOwned(username, itemName);
        }

        [HttpPost("RemoveItemsOwned")]
        public void RemoveItemsOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string itemName)
        {
            DatabaseConnection.ItemsOwnedRepo.RemoveItemsOwned(username, itemName);
        }

        [HttpGet("SelectSingleItemOwned")]
        public string SelectSingleItemOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string itemName)
        {
            ItemsOwned item = DatabaseConnection.ItemsOwnedRepo.SelectSingleItemOwned(username, itemName);
            return JsonSerializer.Serialize(item);
        }

        [HttpGet("TopItem")]
        public string TopItem([FromHeader] string SessionId, [FromQuery] uint year, [FromQuery] uint month)
        {
            IReadOnlyDictionary<uint, uint> items = DatabaseConnection.ItemsOwnedRepo.TopItem(year, month);
            return JsonSerializer.Serialize(items);
        }

        [HttpGet("SelectAllItemOwnedOffset")]
        public string SelectAllItemOwnedOffset([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] int pageNum)
        {
            IReadOnlyList<ItemsOwned> items = DatabaseConnection.ItemsOwnedRepo.SelectAllItemsOwnedByUserOffset(username, (uint)pageNum);
            return JsonSerializer.Serialize(items);
        }

        [HttpGet("GetNumberOfPagesItems")]
        public string GetNumberOfPages([FromHeader] string SessionId, [FromQuery] string username)
        {
            uint pages = DatabaseConnection.ItemsOwnedRepo.SelectAllItemsOwnedByUserOffsetPages(username);
            return JsonSerializer.Serialize(pages);
        }

        /*********************************
         * 
         * ItemTypes Methods
         * 
         * ******************************/

        [HttpGet("api/SelectItemType")]
        public string SelectItemType()
        {
            IReadOnlyList<ItemType> itemTypes = DatabaseConnection.ItemTypeRepo.SelectItemType();
            return JsonSerializer.Serialize(itemTypes);
        }

        [HttpGet("api/AddItemType")]
        public void AddItemType([FromHeader] string SessionId, [FromQuery] string itemTypeName)
        {
            ItemType itemTypes = DatabaseConnection.ItemTypeRepo.AddItemType(itemTypeName);
        }

        /*********************************
         * 
         * Item Methods
         * 
         * ******************************/
        [HttpGet("api/SelectItem")]
        public string SelectItem()
        {
            IReadOnlyList<Item> item = DatabaseConnection.ItemRepo.SelectItem();
            return JsonSerializer.Serialize(item);
        }

        [HttpGet("api/AddItem")]
        public void AddItem([FromHeader] string SessionId, [FromQuery] string itemName, [FromQuery] string description, [FromQuery] string itemTypeName)
        {
            Item item = DatabaseConnection.ItemRepo.AddItem(itemName, description, itemTypeName);
            
        }

    }
}
