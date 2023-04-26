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
            var itemTypeName = "other";
            var itemName = "testItem";
            var description = "WORDSSSS";
            var itemLink = "Link";

            var actual = ItemRepo.AddItem(itemName, description, itemTypeName, itemLink);

            Assert.IsNotNull(actual);
            Assert.That(actual.ItemName, Is.EqualTo(itemName));
            Assert.That(actual.Description, Is.EqualTo(description));
            Assert.That(actual.ItemImageLink, Is.EqualTo(itemLink));
        }

        [Test]
        public void SelectItemWork()
        {
            var itemTypeName = "other";
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
                AssertItemAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        private static void AssertItemAreEqual(Item expected, Item actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.ItemID, Is.EqualTo(expected.ItemID));
            Assert.That(actual.ItemName, Is.EqualTo(expected.ItemName));
            Assert.That(actual.Description, Is.EqualTo(expected.Description));

        }

        private Item CreateTestItem(int a, uint b, string typeName)
        {
            return ItemRepo.AddItem("Test "+ a, "a"+b, typeName, "link"+b);
        }
    }
}
