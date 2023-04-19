using PokemonBox.Models;
using PokemonBox.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBoxTest
{
    public class UtilitiesTest
    {

        [Test]
        public void CryptographyEmptyString_ReturnsEmptyString()
        {
            string result = Cryptography.QuickSHA256Hash("");
            Assert.That(result == "");
        }

        [Test]
        public void CompareCryptography_Success() //test
        {

            string testString = "some string to test";

            string hashresult = Cryptography.QuickSHA256Hash(testString);

            bool result = Cryptography.CompareHash(testString, hashresult);

            Assert.That(result, Is.True);
        }
    }
}
