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
        //[HttpGet]
        //public IEnumerable<string> GetAllOwnedItems() 
        //{
        //
        //}

        [HttpGet("api/GetAllOwnedItemsByUser")]
        public string GetAllOwnedItemsByUser([FromHeader] string SessionId, [FromQuery] string username)
        {
            IReadOnlyList<ItemsOwned> items = DatabaseConnection.ItemsOwnedRepo.SelectAllItemsOwnedByUser(username);
            return JsonSerializer.Serialize(items);
        }

        //[HttpGet]
        //public IEnumerable<string> GetAllItemTypes()
        //{
        //
        //}
    }
}
