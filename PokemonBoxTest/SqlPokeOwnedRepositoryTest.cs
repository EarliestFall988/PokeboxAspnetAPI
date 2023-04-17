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

        private SqlPokeOwnedRepository PokeOwnedRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            PokeOwnedRepo = new SqlPokeOwnedRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void CreatePokeOwnedWork()
        {
            var userName = "TestUser";
            var pokemonName = "TestPokemon";
            var name = "JustAName";
            var gender = pokeGender.unknown;
            var level = (uint)15;

            var actual = PokeOwnedRepo.CreatePokeOwned(userName, pokemonName, name, gender, level);

            Assert.IsNotNull(actual);
            Assert.That(actual.PokeName, Is.EqualTo(name));
            Assert.That(actual.Gender, Is.EqualTo(gender));
            Assert.That(actual.Level, Is.EqualTo(level));
        }

        [Test]
        public void SelectAllPokemonOwnedByUserWork()
        {
            var userName = "ThisTestBud";

            var p1 = CreateTestPokeOwned(userName, "Poke1", "Bob", pokeGender.unknown, 10);
            var p2 = CreateTestPokeOwned(userName, "Poke3", "Gab", pokeGender.unknown, 10);
            var p3 = CreateTestPokeOwned(userName, "Poke3", "Sog", pokeGender.unknown, 10);

            var expected = new Dictionary<string, PokeOwned>
            {
                {p1.PokeName, p1 },
                {p2.PokeName, p2 },
                {p3.PokeName, p3 }
            };

            var actual = PokeOwnedRepo.SelectAllPokemonOwnedByUser(userName);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.PokeName))
                    continue;

                PokeOwned test;
                expected.TryGetValue(a.PokeName, out test);
                AssertPokemonTypeAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        private static void AssertPokemonTypeAreEqual(PokeOwned expected, PokeOwned actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.Level, Is.EqualTo(expected.Level));
            Assert.That(actual.PokeOwnedID, Is.EqualTo(expected.PokeOwnedID));
            Assert.That(actual.UserID, Is.EqualTo(expected.UserID));
            Assert.That(actual.Gender, Is.EqualTo(expected.Gender));
            Assert.That(actual.PokeName, Is.EqualTo(expected.PokeName));
            Assert.That(actual.PokemonID, Is.EqualTo(expected.PokemonID));
        }

        private PokeOwned CreateTestPokeOwned(string userName, string pokemonName, string name, pokeGender gender, uint level)
        {
            return PokeOwnedRepo.CreatePokeOwned(userName, pokemonName, name, gender, level);
        }
    }
}
