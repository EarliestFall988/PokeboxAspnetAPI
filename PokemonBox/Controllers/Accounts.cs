﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using PokemonBox.Models;
using PokemonBox.SqlRepositories;
using PokemonBox.Utils;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokemonBox.Controllers
{
    [EnableCors("*")]
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

        [HttpPost("/api/v1/register")]
        public string Register()
        {

            var userProxy = GetCredentials(true); // C# static typing in the way, had to do some real world 'magic'

            if (userProxy.result)
            {
                string email = userProxy.email;
                string password = userProxy.password;

                if (password.Length < 8)
                    return APIUtilities.InputError("password too short");

                if (password != userProxy.password2)
                    return APIUtilities.InputError("passwords do not match");

                if (!email.Contains('@'))
                    return APIUtilities.InputError("invalid email");

                //do something with the email and password

                //return APIUtilities.Log("e" + email + " p1 " + password + " p2 " + userProxy.password2 + " uname " + userProxy.Username + " admin? " + userProxy.admin + " fname " + userProxy.fName + " lname " + userProxy.lName);
                return APIUtilities.res(200);
            }
            else
            {
                return APIUtilities.ServerError(userProxy.message); //you can add a message in the params or not, up to you
            }
        }

        [HttpPost("/api/v1/login")]
        public string CreateSession([FromBody] string value)
        {
            return ""; //return the guid here
        }

        [HttpDelete("/logout/{sessionId}")]
        public void DeleteSession(string sessionId)
        {
            //may or may not need to implement this...
            // if the session key is stored (from the login) you can delete it here as the user has logged out
        }

        /// <summary>
        /// handle body parsing for the login/register, feel free to copy/modify as requirements/needs change...
        /// </summary>
        /// <returns>results</returns>
        private UserProxyResult GetCredentials(bool register)
        {
            try
            {
                string body = "";

                using (var reader = HttpContext.Request.BodyReader.AsStream())
                {
                    using (StreamReader streamReader = new StreamReader(reader))
                    {
                        body = streamReader.ReadToEnd();
                    }
                }

                string email = "";
                string password = "";

                string password2 = "";
                string uName = "";
                string fName = "";
                string lName = "";
                bool admin = false;

                int success = 0;

                using (var doc = JsonDocument.Parse(body))
                {
                    var root = doc.RootElement.GetProperty("user");

                    success += APIUtilities.TryGetFromProperty(root, "email", out email) == true ? 1 : 0;
                    success += APIUtilities.TryGetFromProperty(root, "password", out password) == true ? 1 : 0;

                    if (register)
                    {
                        success += APIUtilities.TryGetFromProperty(root, "password2", out password2) == true ? 1 : 0;
                        success += APIUtilities.TryGetFromProperty(root, "username", out uName) == true ? 1 : 0;
                        success += APIUtilities.TryGetFromProperty(root, "firstName", out fName) == true ? 1 : 0;
                        success += APIUtilities.TryGetFromProperty(root, "lastName", out lName) == true ? 1 : 0;
                        success += APIUtilities.TryGetFromProperty(root, "admin", out admin) == true ? 1 : 0;
                    }

                    //TODO: hash passwords with bcrypt encryption??

                    if (!register)
                        if (success == 2)
                        {


                            return new UserProxyResult()
                            {
                                result = true,
                                email = email,
                                password = password,
                                message = ""
                            };
                        }
                        else
                        {
                            //failed to get the email and password
                            return new UserProxyResult()
                            {
                                result = false,
                                message = "failed to parse json"
                            };
                        }

                    if (success == 7)
                    {
                        return new UserProxyResult()
                        {
                            result = true,
                            message = "",


                            email = email,
                            password = password,
                            password2 = password2,
                            Username = uName,
                            fName = fName,
                            lName = lName,
                            admin = admin
                        };
                    }
                    else
                    {
                        return new UserProxyResult()
                        {
                            result = false,
                            message = "failed to parse json"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new UserProxyResult()
                {
                    result = false,
                    message = "failed to parse json: " + ex.Message
                };
            }
        }
    }


    /// <summary>
    /// handles the user credentials payload along with extra metadata, properties are self explanitory
    /// </summary>
    public class UserProxyResult
    {
        public string Username { get; set; } = "";
        public string password { get; set; } = "";
        public string password2 { get; set; } = "";
        public string email { get; set; } = "";
        public string fName { get; set; } = "";
        public string lName { get; set; } = "";
        public bool admin { get; set; } = false;

        public bool result { get; set; } = false;
        public string message { get; set; } = "";
    }
}
