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
    public class SqlItemsOwnedRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlItemsOwnedRepository ItemOwnedRepo;
        private SqlItemRepository ItemRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            ItemOwnedRepo = new SqlItemsOwnedRepository(connectionString);
            ItemRepo = new SqlItemRepository(connectionString);
            transaction = new TransactionScope();
        }

        [Test]
        public void AddItemOwnedWork()
        {
            var itemName = "master-ball";
            var userName = "ligula@hotmail.ca";

            var first = ItemOwnedRepo.SelectAllItemsOwned();
            var actual = ItemOwnedRepo.CreateItemsOwned(userName, itemName);
            var second = ItemOwnedRepo.SelectAllItemsOwned();


            Assert.IsNotNull(actual);
            Assert.That(first.Count+1, Is.EqualTo(second.Count));
        }

        [Test]
        public void SelectItemsOwnedWork()
        {
            var itemName = "master-ball";
            var itemName2 = "ultra-ball";
            var itemName3 = "great-ball";
            var userName = "ligula@hotmail.ca";

            var i1 = ItemOwnedRepo.CreateItemsOwned(userName, itemName);
            var i2 = ItemOwnedRepo.CreateItemsOwned(userName, itemName2);
            var i3 = ItemOwnedRepo.CreateItemsOwned(userName, itemName3);

            var expected = new Dictionary<uint, ItemsOwned>
            {
                {i1.ItemOwnedID, i1 },
                {i2.ItemOwnedID, i2 },
                {i3.ItemOwnedID, i3 }
            };

            var actual = ItemOwnedRepo.SelectAllItemsOwned();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.ItemOwnedID))
                    continue;

                ItemsOwned test;
                expected.TryGetValue(a.ItemOwnedID, out test);
                AssertItemsOwnedAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        [Test]
        public void SelectAllItemsOwnedByUserWork()
        {
            var itemName = "master-ball";
            var itemName2 = "ultra-ball";
            var itemName3 = "great-ball";
            var userName = "ante.blandit@hotmail.org";

            var i1 = ItemOwnedRepo.CreateItemsOwned(userName, itemName);
            var i2 = ItemOwnedRepo.CreateItemsOwned(userName, itemName2);
            var i3 = ItemOwnedRepo.CreateItemsOwned(userName, itemName3);

            var expected = new Dictionary<uint, ItemsOwned>
            {
                {i1.ItemOwnedID, i1 },
                {i2.ItemOwnedID, i2 },
                {i3.ItemOwnedID, i3 }
            };

            var actual = ItemOwnedRepo.SelectAllItemsOwnedByUser(userName);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");

            var matchCount = 0;

            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.ItemOwnedID))
                    continue;

                ItemsOwned test;
                expected.TryGetValue(a.ItemOwnedID, out test);
                AssertItemsOwnedAreEqual(test, a);

                matchCount++;
            }

            Assert.That(matchCount, Is.EqualTo(expected.Count));


        }

        [Test]
        public void SelectSingleItemOwnedByUserWork()
        {
            var itemName = "master-ball";
            var itemName2 = "ultra-ball";
            var itemName3 = "great-ball";
            var userName = "in.lorem@outlook.couk";

            var i1 = ItemOwnedRepo.CreateItemsOwned(userName, itemName);
            var i2 = ItemOwnedRepo.CreateItemsOwned(userName, itemName2);
            var i3 = ItemOwnedRepo.CreateItemsOwned(userName, itemName3);

            var expected = new Dictionary<uint, ItemsOwned>
            {
                {i1.ItemOwnedID, i1 },
                {i2.ItemOwnedID, i2 },
                {i3.ItemOwnedID, i3 }
            };

            var actual = ItemOwnedRepo.SelectSingleItemOwned(userName, itemName);

            Assert.IsNotNull(actual);
            AssertItemsOwnedAreEqual(actual, i1);


        }

        private static void AssertItemsOwnedAreEqual(ItemsOwned expected, ItemsOwned actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.ItemID, Is.EqualTo(expected.ItemID));
            Assert.That(actual.UserID, Is.EqualTo(expected.UserID));
            Assert.That(actual.ItemOwnedID, Is.EqualTo(expected.ItemOwnedID));

        }
    }
}
