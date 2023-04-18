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
    public class SqlItemTypeRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlItemTypeRepository ItemTypeRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            ItemTypeRepo = new SqlItemTypeRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void AddItemTypeWork()
        {
            var itemTypeName = "Needs";

            var actual = ItemTypeRepo.AddItemType(itemTypeName);

            Assert.IsNotNull(actual);
            Assert.That(actual.ItemTypeName, Is.EqualTo(itemTypeName));
        }

        [Test]
        public void SelectItemTypeWork()
        {
            var p1 = CreateTestItemType("Test1");
            var p2 = CreateTestItemType("Test2");
            var p3 = CreateTestItemType("Test3");

            var expected = new Dictionary<uint, ItemType>
            {
                {p1.ItemTypeID, p1 },
                {p2.ItemTypeID, p2 },
                {p3.ItemTypeID, p3 }
            };

            var actual = ItemTypeRepo.SelectItemType();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.ItemTypeID))
                    continue;

                ItemType test;
                expected.TryGetValue(a.ItemTypeID, out test);
                AssertItemTypesAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        private static void AssertItemTypesAreEqual(ItemType expected, ItemType actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.ItemTypeID, Is.EqualTo(expected.ItemTypeID));
            Assert.That(actual.ItemTypeName, Is.EqualTo(expected.ItemTypeName));

        }

        private ItemType CreateTestItemType(string name)
        {
            return ItemTypeRepo.AddItemType(name);
        }
    }
}
