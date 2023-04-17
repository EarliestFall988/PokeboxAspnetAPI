using Microsoft.AspNetCore.Mvc;
using PokemonBox.Models;
using PokemonBox.SqlRepositories;
using System.Text.Json;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokemonBox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Accounts : ControllerBase
    {
        private const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlUserRepository _userRepository = new SqlUserRepository(connectionString);
        // GET: api/<Accounts>
        [HttpGet]
        public IEnumerable<string> GetUsers()
        {
            List<string> s = new List<string>();
            IReadOnlyList<Models.User> users = _userRepository.SelectUser();
            foreach (var u in users)
            {
                var str = JsonSerializer.Serialize(u);
                s.Add(str);
            }
            return s;
        }

        // GET api/<Accounts>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Accounts>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Accounts>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Accounts>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
