using PokemonBox.Models;
using PokemonBox.SqlRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PokemonBox.Test
{
    public class SqlPokeOwnedRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlPokeOwnedRepository PokeOwnedRepo;
        private SqlUserRepository UserRepo;
        private SqlPokemonRepository PokemonRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            PokeOwnedRepo = new SqlPokeOwnedRepository(connectionString);
            UserRepo = new SqlUserRepository(connectionString);
            PokemonRepo = new SqlPokemonRepository(connectionString);
            transaction = new TransactionScope();
        }

        [Test]
        public void CreatePokeOwnedWork()
        {
            var userName = "TestUser";
            var pokemonName = "TestPokemon";
            var nickName = "JustAName";
            var gender = pokeGender.unknown;
            var level = (uint)15;

            var user = CreateTestUser(userName, "pass1234", "fName", "LName", false);
            var pokemon = CreateTestPokemon(pokemonName, 45000);

            var actual = PokeOwnedRepo.CreatePokeOwned(userName, pokemonName, nickName, gender, level);

            Assert.IsNotNull(actual);
            Assert.That(actual.NickName, Is.EqualTo(nickName));
            Assert.That(actual.Gender, Is.EqualTo(gender));
            Assert.That(actual.Level, Is.EqualTo(level));
        }

        [Test]
        public void SelectAllPokemonOwnedWork()
        {
            var userName = "TestUserAHHHH";
            var userName2 = "TestUser";
            var pokemonName1 = "Bulbasaur";
            var pokemonName2 = "Bulbasaur";
            var pokemonName3 = "Bulbasaur";


            var user = CreateTestUser(userName, "pass1234", "fName", "LName", false);
            var user2 = CreateTestUser(userName2, "pass1234", "fName", "LName", false);

            var p1 = CreateTestPokeOwned(userName, pokemonName1, "Bob1", pokeGender.unknown, 10);
            var p2 = CreateTestPokeOwned(userName, pokemonName2, "Gab2", pokeGender.unknown, 10);
            var p3 = CreateTestPokeOwned(userName, pokemonName3, "Sog3", pokeGender.unknown, 10);

            var p4 = CreateTestPokeOwned(userName2, pokemonName1, "Bob12", pokeGender.unknown, 10);
            var p5 = CreateTestPokeOwned(userName2, pokemonName2, "Gab22", pokeGender.unknown, 10);
            var p6 = CreateTestPokeOwned(userName2, pokemonName3, "Sog32", pokeGender.unknown, 10);

            var expected = new Dictionary<string, PokeOwned>
            {
                {p1.NickName, p1 },
                {p2.NickName, p2 },
                {p3.NickName, p3 },
                {p4.NickName, p4 },
                {p5.NickName, p5 },
                {p6.NickName, p6 }
            };

            var actual = PokeOwnedRepo.SelectAllPokemonOwned();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 6, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.NickName))
                    continue;

                PokeOwned test;
                expected.TryGetValue(a.NickName, out test);
                AssertPokeOwnedAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        [Test]
        public void SelectAllPokemonOwnedByUserWork()
        {
            var userName = "UGH";
            var pokemonName1 = "Bulbasaur";
            var pokemonName2 = "Bulbasaur";
            var pokemonName3 = "Bulbasaur";


            var user = CreateTestUser(userName, "pass1234", "fName", "LName", false);

            var p1 = CreateTestPokeOwned(userName, pokemonName1, "Bob", pokeGender.unknown, 10);
            var p2 = CreateTestPokeOwned(userName, pokemonName2, "Gab", pokeGender.unknown, 10);
            var p3 = CreateTestPokeOwned(userName, pokemonName3, "Sog", pokeGender.unknown, 10);

            var expected = new Dictionary<string, PokeOwned>
            {
                {p1.NickName, p1 },
                {p2.NickName, p2 },
                {p3.NickName, p3 }
            };

            var actual = PokeOwnedRepo.SelectAllPokemonOwnedByUser(userName);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.NickName))
                    continue;

                PokeOwned test;
                expected.TryGetValue(a.NickName, out test);
                AssertPokeOwnedAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        [Test]
        public void SelectSinglePokeOwnedWork()
        {
            var userName = "TestUser3";
            var pokemonName1 = "Bulbasaur";
            var pokemonName2 = "Bulbasaur";
            var pokemonName3 = "Bulbasaur";


            var user = CreateTestUser(userName, "pass1234", "fName", "LName", false);

            var p1 = CreateTestPokeOwned(userName, pokemonName1, "Bob", pokeGender.unknown, 10);
            var p2 = CreateTestPokeOwned(userName, pokemonName2, "Gab", pokeGender.unknown, 10);
            var p3 = CreateTestPokeOwned(userName, pokemonName3, "Sog", pokeGender.unknown, 10);

            var expected = new Dictionary<string, PokeOwned>
            {
                {p1.NickName, p1 },
                {p2.NickName, p2 },
                {p3.NickName, p3 }
            };

            var actual = PokeOwnedRepo.SelectSinglePokeOwned(userName, pokemonName1, "Bob");

            Assert.IsNotNull(actual);


            Assert.That(actual.NickName, Is.EqualTo("Bob"));
            Assert.That(actual.Gender, Is.EqualTo(pokeGender.unknown));
            Assert.That(actual.Level, Is.EqualTo(10));

        }

        private static void AssertPokeOwnedAreEqual(PokeOwned expected, PokeOwned actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.Level, Is.EqualTo(expected.Level));
            Assert.That(actual.PokeOwnedID, Is.EqualTo(expected.PokeOwnedID));
            Assert.That(actual.UserID, Is.EqualTo(expected.UserID));
            Assert.That(actual.Gender, Is.EqualTo(expected.Gender));
            Assert.That(actual.PokemonID, Is.EqualTo(expected.PokemonID));
        }

        private PokeOwned CreateTestPokeOwned(string userName, string pokemonName, string name, pokeGender gender, uint level)
        {
            return PokeOwnedRepo.CreatePokeOwned(userName, pokemonName, name, gender, level);
        }

        private User CreateTestUser(string userName, string password, string firstName, string lastName, bool admin)
        {
            return UserRepo.AddUser(userName, password, firstName, lastName, admin);
        }

        private Pokemon CreateTestPokemon(string pokemonName, uint dexNum)
        {
            return PokemonRepo.AddPokemon(pokemonName, dexNum, "wordssss" + dexNum, false);
        }
    }
}
