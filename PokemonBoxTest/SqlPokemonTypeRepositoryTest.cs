using System;
using System.Transactions;
using PokemonBox.Models;
using PokemonBox.SqlRepositories;

namespace PokemonBox.Test
{
    public class SqlPokemonTypeRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private static string GetTestString() => Guid.NewGuid().ToString("N");

        private SqlPokemonTypeRepository PokemonTypeRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            PokemonTypeRepo = new SqlPokemonTypeRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void AddPokemonTypeWork()
        {
            var typeName = "Test";

            var actual = PokemonTypeRepo.AddPokemonType(typeName);

            Assert.IsNotNull(actual);
            Assert.That(actual.PokemonTypeName, Is.EqualTo(typeName));
        }

        [Test]
        public void SelectPokemonTypesWork()
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
            return PokemonTypeRepo.AddPokemonType("Test "+a);
        }
    }
}