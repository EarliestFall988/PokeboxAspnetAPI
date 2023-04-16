using System;
using System.Transactions;
using PokemonBox.Models;
using PokemonBox.SqlRepositories;

namespace PokemonBox.Test
{
    public class SqlPokeTypeRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlPokeTypeRepository PokeTypeRepo;
        private SqlPokemonTypeRepository PokemonTypeRepo;
        private SqlPokemonRepository PokemonRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            PokeTypeRepo = new SqlPokeTypeRepository(connectionString);
            PokemonTypeRepo = new SqlPokemonTypeRepository(connectionString);
            PokemonRepo = new SqlPokemonRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void AddPokeTypeWork()
        {
            var type = CreateTestPokemonType(1);
            var poke = CreateTestPokemon(1, 3888);

            var actual = PokeTypeRepo.AddPokeType(type.PokemonTypeName, poke.PokemonName);

            Assert.IsNotNull(actual);
            Assert.That(actual.PokemonTypeID, Is.EqualTo(type.PokemonTypeID));
            Assert.That(actual.PokemonID, Is.EqualTo(poke.PokemonID));
        }

        //[Test]
        //public void SelectPokemonTypesWork()
        //{
        //    var p1 = CreateTestPokemonType(1);
        //    var p2 = CreateTestPokemonType(2);
        //    var p3 = CreateTestPokemonType(3);
        //
        //    var expected = new Dictionary<uint, PokemonType>
        //    {
        //        {p1.PokemonTypeID, p1 },
        //        {p2.PokemonTypeID, p2 },
        //        {p3.PokemonTypeID, p3 }
        //    };
        //
        //    var actual = PokemonTypeRepo.SelectPokemonTypes();
        //
        //    Assert.IsNotNull(actual);
        //    Assert.IsTrue(actual.Count >= 3, "At least three are expected.");
        //
        //    var matchCount = 0;
        //
        //    foreach (var a in actual)
        //    {
        //        if (!expected.ContainsKey(a.PokemonTypeID))
        //            continue;
        //
        //        PokemonType test;
        //        expected.TryGetValue(a.PokemonTypeID, out test);
        //        AssertPokemonTypeAreEqual(test, a);
        //
        //        matchCount++;
        //    }
        //
        //    Assert.That(matchCount, Is.EqualTo(expected.Count));
        //
        //
        //}

        private static void AssertPokeTypeAreEqual(PokeType expected, PokeType actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.PokemonTypeID, Is.EqualTo(expected.PokemonTypeID));
            Assert.That(actual.PokemonID, Is.EqualTo(expected.PokemonID));
        }

        private PokemonType CreateTestPokemonType(int a)
        {
            return PokemonTypeRepo.AddPokemonType("TestType " + a);
        }

        private Pokemon CreateTestPokemon(int a, uint b)
        {
            return PokemonRepo.AddPokemon("TestPoke " + a, b, "wordssss" + a, false);
        }
    }
}
