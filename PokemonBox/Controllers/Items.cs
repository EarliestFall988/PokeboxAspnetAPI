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
            //return JsonSerializer.Serialize(item);
        }

        [HttpPost("RemoveItemsOwned")]
        public void RemoveItemsOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string itemName)
        {
            DatabaseConnection.ItemsOwnedRepo.RemoveItemsOwned(username, itemName);
            //return JsonSerializer.Serialize(item);
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



        /*********************************
         * 
         * ItemTypes Methods
         * 
         * ******************************/

        //[HttpGet("api/GetAllItemTypes")]
        //public string GetAllItemTypes()
        //{
        //    
        //}
    }
}
