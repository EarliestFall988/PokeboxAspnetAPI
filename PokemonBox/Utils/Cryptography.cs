
using System;
using System.Text;
using System.Security;
using System.Security.Cryptography;


namespace PokemonBox.Utils
{
    /// <summary>
    /// Handles quick cryptography algorithms for the databases project, the goal here is to hide our passwords incase we accidentaly use them while demoing our database/project
    /// </summary>
    public class Cryptography
    {

        /// <summary>
        /// Returns a quick hashed string. Should not be used for user passwords, but can be used to hide data and help with things like checksums... etc...
        /// </summary>
        /// <remarks>
        /// Here I am just adding this for user passwords since this is not really apart of the project scope
        /// </remarks>
        /// <param name="input">the input string</param>
        /// <returns>returns the hashed result</returns>
        public static string QuickSHA256Hash(string input)
        {
            if (input == string.Empty)
                return string.Empty;

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }

        /// <summary>
        /// Compare an input string to a existing SHA256 hash for equality
        /// </summary>
        /// <param name="input">the input string</param>
        /// <param name="comparedHash">the hash</param>
        /// <returns>returns true if the hashes are the same, false if not</returns>
        public static bool CompareHash(string input, string comparedHash)
        {
            var result = QuickSHA256Hash(input);
            return result == comparedHash;
        }
    }
}
