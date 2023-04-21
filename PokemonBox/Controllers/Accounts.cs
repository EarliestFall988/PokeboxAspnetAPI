using Microsoft.AspNetCore.Cors;
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
        #region ADMIN stuff

        // GET: api/<Accounts>
        [HttpGet]
        public string GetUsers()
        {
            //List<string> s = new List<string>();
            IReadOnlyList<Models.User> users = DatabaseConnection.UserRepo.SelectUser();
            //foreach (var u in users)
            //{
            //    var str = ;
            //    s.Add(str);
            //}
            return JsonSerializer.Serialize(users);
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

        #endregion

        [HttpPost("/api/v1/register")]
        public string Register()
        {

            var userProxy = GetCredentials(true); // C# static typing in the way, had to do some real world 'magic'

            if (userProxy.result)
            {
                string email = userProxy.email;
                string unhashedPassword = userProxy.password;

                if (unhashedPassword.Length < 8)
                    return APIUtilities.InputError("password too short");

                if (unhashedPassword != userProxy.password2)
                    return APIUtilities.InputError("passwords do not match");

                if (!email.Contains('@'))
                    return APIUtilities.InputError("invalid email");

                var password = Cryptography.QuickSHA256Hash(unhashedPassword); //i know this is not secure, just for obfuscation (maybe brownie points) ... 

                //do something with the email and password

                return APIUtilities.res(200);
            }
            else
            {
                return APIUtilities.ServerError(userProxy.message); //you can add a message in the params or not, up to you
            }
        }

        [HttpPost("/api/v1/login")]
        public string CreateSession()
        {

            var userProxy = GetCredentials(false); // C# static typing in the way, had to do some real world 'magic'

            if (userProxy.result)
            {
                string email = userProxy.email;
                string unhashedPassword = userProxy.password;

                var password = Cryptography.QuickSHA256Hash(unhashedPassword); //i know this is not secure, just for obfuscation

                //do something with the email and password here



                string uid = Guid.NewGuid().ToString(); //creating a session key
                DatabaseConnection.Sessions.Add(uid, email); // adding users to the list of loggedin users, this should probably be time stamped, and stored the database



                return APIUtilities.CreateSession(email, uid); // I need to return more data than just the email...
            }
            else
            {
                return APIUtilities.ServerError("proxy error message: " + userProxy.message); //you can add a message in the params or not, up to you
            }
        }

        [HttpGet("/api/v1/sessions")]
        public IEnumerable<string> GetSessions()
        {
            return DatabaseConnection.Sessions.Keys;
        }


        [HttpDelete("/api/v1/logout")]
        public string DeleteSession([FromQuery] string sessionId)
        {
            //if (LoggedInUsersTempDict.ContainsKey(sessionId))
            //{
            //    LoggedInUsersTempDict.Remove(sessionId);

            //    return APIUtilities.OK();
            //}
            //else
            //{
            //    return APIUtilities.BadRequest(); // enumeration attack could happen here, should be replaced probably with an ok, even if it fails
            //}

            if (DatabaseConnection.Sessions.ContainsKey(sessionId))
            {
                DatabaseConnection.Sessions.Remove(sessionId);
            }


            return APIUtilities.OK();
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

                    //Debug.WriteLine("/n/n/ntest/n/n/n");

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
