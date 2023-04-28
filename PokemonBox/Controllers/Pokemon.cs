using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using PokemonBox.Models;
using PokemonBox.SqlRepositories;
using PokemonBox.Utils;

using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace PokemonBox.Controllers
{
    [EnableCors("*")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Pokemon : ControllerBase
    {
        /*********************************
        * 
        * Pokemon Methods
        * 
        * ******************************/
        [HttpGet("SelectPokemon")]
        public string SelectPokemon()
        {
            IReadOnlyList<Models.Pokemon> pokemon = DatabaseConnection.PokemonRepo.SelectPokemon();
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpPost("AddPokemon")]
        public string AddPokemon([FromHeader] string SessionId, [FromQuery] string pokemonName, [FromQuery] string pokedexNumber, [FromQuery] string imageLink, [FromQuery] string isLegendary)
        {
            var str = GetValidPokemonAdd(pokemonName, int.Parse(pokedexNumber), imageLink);
            if(str.Equals("Valid"))
            {
                Models.Pokemon pokemon = DatabaseConnection.PokemonRepo.AddPokemon(pokemonName, uint.Parse(pokedexNumber), imageLink, bool.Parse(isLegendary));
                return JsonSerializer.Serialize(pokemon);
            }
            else
            {
                return JsonSerializer.Serialize(str);
            }
        }

        /*********************************
        * 
        * PokemonOwned Methods
        * 
        * ******************************/
        [HttpPost("CreatePokeOwned")]
        public string CreatePokeOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string pokemonName, [FromQuery] string name, [FromQuery] pokeGender gender, [FromQuery] uint level)
        {
            var str = GetValidPokeOwnedAdd(username, pokemonName, name);
            if(str.Equals("Valid"))
            {
                PokeOwned pokemon = DatabaseConnection.PokeOwnedRepo.CreatePokeOwned(username, pokemonName, name, gender, level);
                return JsonSerializer.Serialize(pokemon);
            }
            else
            {
                return JsonSerializer.Serialize(str);
            }
        }

        [HttpPost("RemovePokeOwned")]
        public void RemovePokeOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string pokemonName, [FromQuery] string name)
        {
            DatabaseConnection.PokeOwnedRepo.RemovePokeOwned(username, pokemonName, name);
        }

        [HttpGet("SelectAllPokemonOwnedByUser")]
        public string SelectAllPokemonOwnedByUser([FromHeader] string SessionId, [FromQuery] string username)
        {
            IReadOnlyList<PokeOwned> pokemon = DatabaseConnection.PokeOwnedRepo.SelectAllPokemonOwnedByUser(username);
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("SelectAllPokemonOwned")]
        public string SelectAllPokemonOwned()
        {
            IReadOnlyList<PokeOwned> pokemon = DatabaseConnection.PokeOwnedRepo.SelectAllPokemonOwned();
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("SelectSinglePokemonOwned")]
        public string SelectSinglePokeOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string pokemonName, [FromQuery] string name)
        {
            PokeOwned pokemon = DatabaseConnection.PokeOwnedRepo.SelectSinglePokeOwned(username, pokemonName, name);
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("AverageLevel")]
        public string AverageLevel()
        {
            IReadOnlyDictionary<string, decimal> pokemon = DatabaseConnection.PokeOwnedRepo.AverageLevel();
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("PokeRank")]
        public string PokeRank([FromHeader] string SessionId, [FromQuery] string pokemonName)
        {
            IReadOnlyList<PokeRank> pokemon = DatabaseConnection.PokeOwnedRepo.PokeRank(pokemonName);
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("PokeTypeCount")]
        public string PokeTypeCount([FromHeader] string SessionId, [FromQuery] string startMonth, string startYear, [FromQuery] string endMonth, [FromQuery] string endYear)
        {
            var start = new DateTime(int.Parse(startYear), int.Parse(startMonth), 1);
            var end = new DateTime(int.Parse(endYear), int.Parse(endMonth), 28);
            
            IReadOnlyDictionary<string, uint> pokemon = DatabaseConnection.PokeOwnedRepo.PokeTypeCount(DateTime.SpecifyKind(start, DateTimeKind.Utc), DateTime.SpecifyKind(end, DateTimeKind.Utc));
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("SelectAllPokeOwnedOffset")]
        public string SelectAllPokeOwnedOffset([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string pageNum)
        {
            uint.TryParse(pageNum, out uint page);
            IReadOnlyList<PokeOwnedPresentation> pokemon = DatabaseConnection.PokeOwnedRepo.SelectAllPokemonOwnedByUserPages(username, page);
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("GetNumberOfPagesPokeOwned")]
        public string GetNumberOfPages([FromHeader] string SessionId, [FromQuery] string username)
        {
            uint pages = DatabaseConnection.PokeOwnedRepo.SelectAllPokemonOwnedByUserNumberPages(username);
            return JsonSerializer.Serialize(pages);
        }

        /*********************************
        * 
        * PokemonType Methods
        * 
        * ******************************/
        [HttpPost("AddPokemonType")]
        public string AddPokemonType([FromHeader] string SessionId, [FromQuery] string typeName)
        {
            var str = GetValidPokemonTypeAdd(typeName);
            if (str.Equals("Valid"))
            {
                PokemonType type = DatabaseConnection.PokemonTypeRepo.AddPokemonType(typeName);
                return JsonSerializer.Serialize(type);
            }
            else
            {
                return JsonSerializer.Serialize(str);
            }
            
        }

        [HttpGet("SelectPokemonTypes")]
        public string SelectPokemonTypes()
        {
            IReadOnlyList<PokemonType> pokemon = DatabaseConnection.PokemonTypeRepo.SelectPokemonTypes();
            return JsonSerializer.Serialize(pokemon);
        }

        /*********************************
        * 
        * PokeType Methods
        * 
        * ******************************/
        [HttpPost("AddPokeType")]
        public void AddPokeType([FromHeader] string SessionId, [FromQuery] string pokemonTypeName, [FromQuery] string pokemonName)
        {
            PokeType type = DatabaseConnection.PokeTypeRepo.AddPokeType(pokemonTypeName, pokemonName);
        }

        [HttpGet("SelectPokeType")]
        public string SelectPokeType()
        {
            IReadOnlyList<PokeType> pokemon = DatabaseConnection.PokeTypeRepo.SelectPokeType();
            return JsonSerializer.Serialize(pokemon);
        }

        /*********************************
        * 
        * Helper Methods
        * 
        * ******************************/
        private string GetValidPokemonAdd(string pokemonName, int pokedex, string imageLink)
        {
            if (pokemonName.Length > 0)
            {
                return APIUtilities.InputError("MUST HAVE POKEMON NAME");
            }
            if (imageLink.Length > 0)
            {
                return APIUtilities.InputError("MUST HAVE IMAGE LINK");
            }
            
            IReadOnlyList<Models.Pokemon> pokemon = DatabaseConnection.PokemonRepo.SelectPokemon();
            foreach(var p in pokemon)
            {
                if (p.PokemonName.Equals(pokemonName))
                {
                    return APIUtilities.InputError("CANNOT ADD POKEMON THERE EXISTS ONE WITH SAME NAME");
                }
                if (p.PokedexNumber == pokedex)
                {
                    return APIUtilities.InputError("CANNOT ADD POKEMON THERE EXISTS ONE WITH SAME POKEDEX");
                }
            }
            return "Valid";
        }

        private string GetValidPokeOwnedAdd(string username, string pokemonName, string nickname)
        {
            if (!(pokemonName.Length > 0))
            {
                return APIUtilities.InputError("MUST HAVE POKEMON NAME");
            }
            if (!(nickname.Length > 0))
            {
                return APIUtilities.InputError("MUST HAVE NICKNAME");
            }
            IReadOnlyList<PokeOwned> pokemon = DatabaseConnection.PokeOwnedRepo.SelectAllPokemonOwnedByUser(username);
            foreach (var p in pokemon)
            {
                Tuple<string, string> words = DatabaseConnection.PokeOwnedRepo.FetchPokemonOwned(username, p.PokeOwnedID);
                if (words.Item1.Equals(pokemonName) && words.Item2.Equals(nickname))
                {
                    return APIUtilities.InputError("CANNOT ADD POKEMON THERE EXISTS ONE WITH SAME NICKNAME");
                }
            }
            IReadOnlyList<Models.Pokemon> checkPokemon = DatabaseConnection.PokemonRepo.SelectPokemon();
            bool check = false;
            foreach (var p in checkPokemon)
            {
                if(pokemonName.Equals(p.PokemonName))
                {
                    check = true;
                }
            }
            if(!check)
            {
                return APIUtilities.InputError("CANNOT ADD THERE IS NO SUCH POKEMON");
            }
            return "Valid";
        }

        private string GetValidPokemonTypeAdd(string pokemonTypeName)
        {
            IReadOnlyList<PokemonType> pokemon = DatabaseConnection.PokemonTypeRepo.SelectPokemonTypes();
            if (pokemonTypeName.Length > 0)
            {
                return APIUtilities.InputError("MUST HAVE POKEMON TYPE NAME");
            }
            foreach (var p in pokemon)
            {
                if (p.PokemonTypeName.Equals(pokemonTypeName))
                {
                    return APIUtilities.InputError("CANNOT ADD POKEMONTYPE THERE EXISTS ONE WITH SAME NAME");
                }
            }
            return "Valid";
        }



    }
}
