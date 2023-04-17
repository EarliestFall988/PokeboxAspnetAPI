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
    public class SqlItemRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlItemRepository ItemRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            ItemRepo = new SqlItemRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void AddItemWork()
        {
            var itemName = "Sleep";
            var description = "cool thing";
            var itemTypeName = "Needs";

            var actual = ItemRepo.AddItem(itemName, description, itemTypeName);

            Assert.IsNotNull(actual);
            Assert.That(actual.ItemName, Is.EqualTo(itemName));
            Assert.That(actual.Description, Is.EqualTo(description));
        }

        [Test]
        public void SelectItemWork()
        {
            var itemTypeName = "needs";
            var p1 = CreateTestItem(1, 0, itemTypeName);
            var p2 = CreateTestItem(2, 1, itemTypeName);
            var p3 = CreateTestItem(3, 2, itemTypeName);

            var expected = new Dictionary<uint, Item>
            {
                {p1.ItemID, p1 },
                {p2.ItemID, p2 },
                {p3.ItemID, p3 }
            };

            var actual = ItemRepo.SelectItem();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.ItemID))
                    continue;

                Item test;
                expected.TryGetValue(a.ItemID, out test);
                AssertPokemonAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        private static void AssertPokemonAreEqual(Item expected, Item actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.ItemID, Is.EqualTo(expected.ItemID));
            Assert.That(actual.ItemName, Is.EqualTo(expected.ItemName));
            Assert.That(actual.Description, Is.EqualTo(expected.Description));

            //Assert.That(actual.DateAdded.ToString("MM / dd / yyyy hh: mm:ss"), Is.EqualTo(expected.DateAdded.ToString("MM / dd / yyyy hh: mm:ss")));

        }

        private Item CreateTestItem(int a, uint b, string typeName)
        {
            return ItemRepo.AddItem("Test "+ a, "a"+b, typeName);
        }
    }
}
