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
    public class SqlPokemonRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlPokeTypeRepository PokeTypeRepo;
        private SqlPokemonRepository PokemonRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            PokeTypeRepo = new SqlPokeTypeRepository(connectionString);
            PokemonRepo = new SqlPokemonRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void AddPokemonWork()
        {
            var pokemonName = "Jacob";
            int pokedexNum = 5000;
            var description = "Big poo poo head";
            var isLegendary = false;

            var actual = PokemonRepo.AddPokemon(pokemonName, (uint)pokedexNum, description, isLegendary);

            Assert.IsNotNull(actual);
            Assert.That(actual.PokemonName, Is.EqualTo(pokemonName));
            Assert.That(actual.PokedexNumber, Is.EqualTo(pokedexNum));
            Assert.That(actual.Description, Is.EqualTo(description));
            Assert.That(actual.IsLegendary, Is.EqualTo(isLegendary));
        }

        [Test]
        public void SelectPokemonWork()
        {
            var p1 = CreateTestPokemon(1, 4000);
            var p2 = CreateTestPokemon(2, 4001);
            var p3 = CreateTestPokemon(3, 4002);
        
            var expected = new Dictionary<uint, Pokemon>
            {
                {p1.PokemonID, p1 },
                {p2.PokemonID, p2 },
                {p3.PokemonID, p3 }
            };
        
            var actual = PokemonRepo.SelectPokemon();
        
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");
        
            var matchCount = 0;
        
            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.PokemonID))
                    continue;
        
                Pokemon test;
                expected.TryGetValue(a.PokemonID, out test);
                AssertPokemonAreEqual(test, a);
        
                matchCount++;
            }
        
            Assert.That(matchCount, Is.EqualTo(expected.Count));
        
        
        }
        
        private static void AssertPokemonAreEqual(Pokemon expected, Pokemon actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.PokedexNumber, Is.EqualTo(expected.PokedexNumber));
            Assert.That(actual.PokemonID, Is.EqualTo(expected.PokemonID));
            Assert.That(actual.IsLegendary, Is.EqualTo(expected.IsLegendary));
            //Assert.That(actual.DateAdded.ToString("MM / dd / yyyy hh: mm:ss"), Is.EqualTo(expected.DateAdded.ToString("MM / dd / yyyy hh: mm:ss")));
            Assert.That(actual.PokemonName, Is.EqualTo(expected.PokemonName));
            Assert.That(actual.Description, Is.EqualTo(expected.Description));
        }
        
        private Pokemon CreateTestPokemon(int a, uint b)
        {
            return PokemonRepo.AddPokemon("Test " + a, b, "wordssss"+a, false );
        }
    }
}
