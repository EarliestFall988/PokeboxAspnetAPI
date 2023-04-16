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

        [Test]
        public void SelectPokemonTypesWork()
        {
            var t1 = CreateTestPokemonType(10);
            var t2 = CreateTestPokemonType(20);
            var t3 = CreateTestPokemonType(30);

            var p1 = CreateTestPokemon(10, 3889);
            var p2 = CreateTestPokemon(20, 3890);
            var p3 = CreateTestPokemon(30, 3891);

            var pt1 = PokeTypeRepo.AddPokeType(t1.PokemonTypeName, p1.PokemonName);
            var pt2 = PokeTypeRepo.AddPokeType(t2.PokemonTypeName, p2.PokemonName);
            var pt3 = PokeTypeRepo.AddPokeType(t3.PokemonTypeName, p3.PokemonName);

            var expected = new Dictionary<uint, PokeType>
            {
                {pt1.PokemonTypeID, pt1 },
                {pt2.PokemonTypeID, pt2 },
                {pt3.PokemonTypeID, pt3 }
            };
        
            var actual = PokeTypeRepo.SelectPokeType();
        
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");
        
            var matchCount = 0;
        
            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.PokemonTypeID))
                    continue;
        
                PokeType test;
                expected.TryGetValue(a.PokemonTypeID, out test);
                //if(a.PokemonID.Equals(test.PokemonID))
                //{
                    matchCount++;
            }
        
            Assert.That(matchCount, Is.EqualTo(expected.Count));
        
        
        }

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
