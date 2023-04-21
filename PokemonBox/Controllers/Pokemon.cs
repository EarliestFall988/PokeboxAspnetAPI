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

        [HttpGet("PokeTypeCount")]
        public string PokeTypeCount()
        {
            IReadOnlyDictionary<Models.Pokemon, PokeType> pokemon = DatabaseConnection.PokemonRepo.PokeTypeCount();
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpPost("AddPokemon")]
        public void AddPokemon([FromHeader] string SessionId, [FromQuery] string pokemonName, [FromQuery] uint pokedexNumber, [FromQuery] string description, [FromQuery] bool isLegendary)
        {
            Models.Pokemon pokemon = DatabaseConnection.PokemonRepo.AddPokemon(pokemonName, pokedexNumber, description, isLegendary);
        }

        /*********************************
        * 
        * PokemonOwned Methods
        * 
        * ******************************/
        [HttpPost("CreatePokeOwned")]
        public void CreatePokeOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string pokemonName, [FromQuery] string name, [FromQuery] pokeGender gender, [FromQuery] uint level)
        {
            PokeOwned pokemon = DatabaseConnection.PokeOwnedRepo.CreatePokeOwned(username, pokemonName, name, gender, level);
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

        [HttpGet("SelectAllPokemonOwned")]
        public string SelectSinglePokeOwned([FromHeader] string SessionId, [FromQuery] string username, [FromQuery] string pokemonName, [FromQuery] string name)
        {
            PokeOwned pokemon = DatabaseConnection.PokeOwnedRepo.SelectSinglePokeOwned(username, pokemonName, name);
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("AverageLevel")]
        public string AverageLevel()
        {
            IReadOnlyDictionary<uint, decimal> pokemon = DatabaseConnection.PokeOwnedRepo.AverageLevel();
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("PokeRank")]
        public string PokeRank([FromHeader] string SessionId, [FromQuery] string pokemonName)
        {
            IReadOnlyDictionary<uint, uint> pokemon = DatabaseConnection.PokeOwnedRepo.PokeRank(pokemonName);
            return JsonSerializer.Serialize(pokemon);
        }

        [HttpGet("PokeTypeCount")]
        public string PokeTypeCount([FromHeader] string SessionId, [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
        {
            IReadOnlyDictionary<string, uint> pokemon = DatabaseConnection.PokeOwnedRepo.PokeTypeCount(start, end);
            return JsonSerializer.Serialize(pokemon);
        }

        /*********************************
        * 
        * PokemonType Methods
        * 
        * ******************************/
        [HttpPost("AddPokemonType")]
        public void AddPokemonType([FromHeader] string SessionId, [FromQuery] string typeName)
        {
            PokemonType type = DatabaseConnection.PokemonTypeRepo.AddPokemonType(typeName);
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

    }
}
