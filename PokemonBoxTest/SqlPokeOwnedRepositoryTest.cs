using PokemonBox.Models;
using PokemonBox.SqlRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PokemonBox.Test
{
    public class SqlPokeOwnedRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlPokemonTypeRepository PokemonTypeRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            PokemonTypeRepo = new SqlPokemonTypeRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void CreatePokeOwnedWork()
        {
            var typeName = "Test";

            var actual = PokemonTypeRepo.AddPokemonType(typeName);

            Assert.IsNotNull(actual);
            Assert.That(actual.PokemonTypeName, Is.EqualTo(typeName));
        }

        [Test]
        public void RemovePokeOwnedWork()
        {
            var p1 = CreateTestPokemonType(1);
            var p2 = CreateTestPokemonType(2);
            var p3 = CreateTestPokemonType(3);

            var expected = new Dictionary<uint, PokemonType>
            {
                {p1.PokemonTypeID, p1 },
                {p2.PokemonTypeID, p2 },
                {p3.PokemonTypeID, p3 }
            };

            var actual = PokemonTypeRepo.SelectPokemonTypes();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.PokemonTypeID))
                    continue;

                PokemonType test;
                expected.TryGetValue(a.PokemonTypeID, out test);
                AssertPokemonTypeAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        private static void AssertPokemonTypeAreEqual(PokemonType expected, PokemonType actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.PokemonTypeName, Is.EqualTo(expected.PokemonTypeName));
            Assert.That(actual.PokemonTypeID, Is.EqualTo(expected.PokemonTypeID));
        }

        private PokemonType CreateTestPokemonType(int a)
        {
            return PokemonTypeRepo.AddPokemonType("Test " + a);
        }
    }
}
