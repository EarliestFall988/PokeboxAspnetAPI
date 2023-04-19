
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace PokemonBox.Utils
{
    /// <summary>
    /// Great place for functions, structs, classes and methods that don't have a home
    /// </summary>
    public class APIUtilities
    {
        /// <summary>
        /// Convert a dictionary of values into a json stirng
        /// </summary>
        /// <param name="dict">the dictionary</param>
        /// <returns>return the json string</returns>
        public static string toJson(Dictionary<string, string> dict)
        {
            return JsonSerializer.Serialize<Dictionary<string, string>>(dict);
        }


        /// <summary>
        /// Return a request that went through with no problems
        /// </summary>
        /// <returns></returns>
        public static string OK()
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("message", "ok");
            dict.Add("status", "200");

            return toJson(dict);
        }

        /// <summary>
        /// return a result with the server error code
        /// </summary>
        /// <param name="serverError">can provide more details about why the server failed</param>
        /// <returns></returns>
        public static string ServerError(string? serverError = null)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (serverError != null)
            {
                dict.Add("message", serverError.ToString());
            }
            else
            {
                dict.Add("message", "server error");
            }

            dict.Add("status", "500");

            return toJson(dict);
        }


        /// <summary>
        /// the requested resource was not found
        /// </summary>
        /// <returns></returns>
        public static string NotFound()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("message", "Not found");
            dict.Add("status", "404");

            return toJson(dict);
        }

        /// <summary>
        /// bad request
        /// </summary>
        /// <returns></returns>
        public static string BadRequest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("message", "bad request");
            dict.Add("status", "400");

            return toJson(dict);
        }

        /// <summary>
        /// Create a session after a user has logged in
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string CreateSession(string uid)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("session", uid);
            dict.Add("status", "200");

            return toJson(dict);
        }

        public static string Custom(string message, int status)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("message", message);
            dict.Add("status", status.ToString());

            return toJson(dict);
        }

        public static string Log(string message)
        {
            return Custom(message, 200);
        }

        public static string InputError(string message)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("err", message);
            dict.Add("status", "400");

            return toJson(dict);
        }

        public static string res(int type)
        {
            switch (type)
            {
                case 200:
                    return OK();
                case 500:
                    return ServerError();

                case 404:
                    return NotFound();
                case 400:
                    return BadRequest();

                default:
                    return ServerError("the type of server response " + type + "was not found");
            }
        }

        public static bool TryGetFromProperty(JsonElement e, string propertyName, out string result)
        {
            if (e.TryGetProperty(propertyName, out var value))
            {
                string? str = value.GetString();

                if (str != null)
                {
                    result = str;
                    return true;
                }
            }

            result = "";
            return false;
        }

        public static bool TryGetFromProperty(JsonElement e, string propertyName, out bool result)
        {
            if (e.TryGetProperty(propertyName, out var value))
            {
                var str = value.GetBoolean();

                result = str;
                return true;
            }

            result = false;
            return false;
        }
    }
}
