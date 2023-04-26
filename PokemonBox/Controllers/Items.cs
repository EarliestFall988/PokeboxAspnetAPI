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
        public string CreateItemsOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string itemName)
        {
            var str = GetValidItemOwnedAdd(username, itemName);
            if (str.Equals("Valid"))
            {
                ItemsOwned item = DatabaseConnection.ItemsOwnedRepo.CreateItemsOwned(username, itemName);
                return JsonSerializer.Serialize(item);
            }
            else
            {
                JsonSerializer.Serialize(str);
            }
            
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

        [HttpGet("SelectItemType")]
        public string SelectItemType()
        {
            IReadOnlyList<ItemType> itemTypes = DatabaseConnection.ItemTypeRepo.SelectItemType();
            return JsonSerializer.Serialize(itemTypes);
        }

        [HttpGet("AddItemType")]
        public string AddItemType([FromHeader] string SessionId, [FromQuery] string itemTypeName)
        {
            var str = GetValidItemTypeAdd(itemTypeName);
            if (str.Equals("Valid"))
            {
                ItemType itemTypes = DatabaseConnection.ItemTypeRepo.AddItemType(itemTypeName);
                return JsonSerializer.Serialize(itemTypes);
            }
            else
            {
                JsonSerializer.Serialize(str);
            }
            
        }

        /*********************************
         * 
         * Item Methods
         * 
         * ******************************/
        [HttpGet("SelectItem")]
        public string SelectItem()
        {
            IReadOnlyList<Item> item = DatabaseConnection.ItemRepo.SelectItem();
            return JsonSerializer.Serialize(item);
        }

        [HttpGet("AddItem")]
        public string AddItem([FromHeader] string SessionId, [FromQuery] string itemName, [FromQuery] string description, [FromQuery] string itemTypeName, [FromQuery] string itemImageLink)
        {
            var str = GetValidItemAdd(itemName, description, itemImageLink, itemTypeName);
            if(str.Equals("Valid"))
            {
                Item item = DatabaseConnection.ItemRepo.AddItem(itemName, description, itemTypeName, itemImageLink);
                return JsonSerializer.Serialize(item);
            }
            else
            {
                return JsonSerializer.Serialize(str);
            }
            
        }

        /*********************************
        * 
        * Helper Methods
        * 
        * ******************************/
        private string GetValidItemAdd(string itemName, string description, string imageLink, string itemTypeName)
        {
            IReadOnlyList<Item> item = DatabaseConnection.ItemRepo.SelectItem();
            foreach (var i in item)
            {
                if(itemName.Length > 0)
                {
                    return APIUtilities.InputError("MUST HAVE ITEM NAME");
                }
                if(imageLink.Length > 0)
                {
                    return APIUtilities.InputError("MUST HAVE IMAGE LINK");
                }
                if(description.Length > 0)
                {
                    return APIUtilities.InputError("MUST HAVE DESCRIPTION");
                }
                if (itemTypeName.Length > 0)
                {
                    return APIUtilities.InputError("MUST HAVE ITEM TYPE NAME");
                }
                IReadOnlyList<ItemType> itemTypes = DatabaseConnection.ItemTypeRepo.SelectItemType();
                foreach(var it in itemTypes) 
                {
                    if(!it.ItemTypeName.Equals(itemTypeName))
                    {
                        return APIUtilities.InputError("MUST HAVE VALID ITEM TYPE NAME");
                    }
                }
                if (i.ItemName.Equals(itemName))
                {
                    return APIUtilities.InputError("CANNOT ADD ITEM THERE EXISTS ONE WITH SAME NAME");
                }
            }
            return "Valid";
        }

        private string GetValidItemOwnedAdd(string username, string itemName)
        {
            IReadOnlyList<ItemsOwned> items = DatabaseConnection.ItemsOwnedRepo.SelectAllItemsOwnedByUser(username);
            foreach (var i in items)
            {
                var id = DatabaseConnection.ItemsOwnedRepo.FetchItemOwned(username, i.ItemOwnedID);
                if (itemName.Length > 0)
                {
                    return APIUtilities.InputError("MUST HAVE ITEM NAME");
                }
                if (i.ItemID == id)
                {
                    return APIUtilities.InputError("CANNOT ADD ITEM THERE EXISTS ONE WITH SAME NAME");
                }
            }
            return "Valid";
        }

        private string GetValidItemTypeAdd(string itemTypeName)
        {
            IReadOnlyList<ItemType> itemTypes = DatabaseConnection.ItemTypeRepo.SelectItemType();
            foreach (var i in itemTypes)
            {
                if (itemTypeName.Length > 0)
                {
                    return APIUtilities.InputError("MUST HAVE ITEM TYPE NAME");
                }
                if (i.ItemTypeName.Equals(itemTypeName))
                {
                    return APIUtilities.InputError("CANNOT ADD ITEMTYPE THERE EXISTS ONE WITH SAME NAME");
                }
            }
            return "Valid";
        }

    }
}
