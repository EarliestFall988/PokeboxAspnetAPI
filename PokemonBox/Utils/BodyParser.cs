using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokemonBox.Utils
{
    public class BodyParser
    {


        [Obsolete("do not use for the CIS 560 project")]
        public static string Parse(HttpContext ctx)
        {
            string body = "";

            using (var reader = ctx.Request.BodyReader.AsStream())
            {
                using (StreamReader streamReader = new StreamReader(reader))
                {
                    body = streamReader.ReadToEnd();
                }
            }

            return body;
        }

        [Obsolete("do not use for the CIS 560 project")]
        public static bool GetValues(string body, List<(JsonProperty prop, string name, DataTypes types)> input, out Dictionary<string, object> res, string? rootname = null)
        {
            //ope trying to abstract too much, probably would be okay if this was a 6 month proj

            res = new Dictionary<string, object>();
            return false;

            //    int success = 0;

            //    int successTotal = input.Count;

            //    Dictionary<string, object> result = new Dictionary<string, object>();

            //    using (var doc = JsonDocument.Parse(body))
            //    {
            //        var root = doc.RootElement;

            //        if (rootname != null)
            //        {
            //            root = doc.RootElement.GetProperty(rootname);
            //        }

            //        success +=
            //        //success += APIUtilities.TryGetStringFromProperty(root, "password", out password) == true ? 1 : 0;

            //        foreach (var x in input)
            //        {
            //            if (x.types == DataTypes.StringType)
            //            {
            //                if(APIUtilities.TryGetStringFromProperty(root, "email", out var strResult))
            //                {

            //                }
            //            }
            //        }

            //        if (success == 2)
            //        {
            //            //do something with the email and password

            //            return APIUtilities.OK();
            //        }
            //        else
            //        {
            //            //failed to get the email and password
            //            return APIUtilities.res(500);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// The data types supported by the body parser
        /// </summary>
        public enum DataTypes
        {
            StringType,
            IntType,
            BoolType,
            DoubleType
        }
    }
}
