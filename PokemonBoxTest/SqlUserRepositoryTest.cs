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
    public class SqlUserRepositoryTest
    {
        const string connectionString = @"Server=(localdb)\MSSQLLocalDb;Database=PokemonBoxDatabase;Integrated Security=SSPI;";

        private SqlUserRepository UserRepo;
        private TransactionScope transaction;

        [SetUp]
        public void Setup()
        {
            UserRepo = new SqlUserRepository(connectionString);

            transaction = new TransactionScope();
        }

        [Test]
        public void AddUserWork()
        {
            var userName = "TestUser";
            var password = "TestPassword";
            var firstName = "JustAFirstName";
            var lastName = "JustALastName";
            var admin = false;

            var actual = UserRepo.AddUser(userName, password, firstName,lastName, admin);

            Assert.IsNotNull(actual);
            Assert.That(actual.UserName, Is.EqualTo(userName));
            Assert.That(actual.Password, Is.EqualTo(password));
            Assert.That(actual.FirstName, Is.EqualTo(firstName));
            Assert.That(actual.LastName, Is.EqualTo(lastName));
            Assert.That(actual.IsAdmin, Is.EqualTo(admin));
        }

        [Test]
        public void SelectUserWork()
        {
            var userName = "ThisTestBud";
        
            var p1 = CreateTestUser(userName + "1", "Poke1", "Bob", "Bob", false);
            var p2 = CreateTestUser(userName+ "2", "Poke3", "Gab", "Gab", true);
            var p3 = CreateTestUser(userName + "3", "Poke3", "Sog", "Sog", false);
        
            var expected = new Dictionary<string, User>
            {
                {p1.UserName, p1 },
                {p2.UserName, p2 },
                {p3.UserName, p3 }
            };
        
            var actual = UserRepo.SelectUser();
        
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 3, "At least three are expected.");
        
            var matchCount = 0;
        
            foreach (var a in actual)
            {
                if (!expected.ContainsKey(a.UserName))
                    continue;
        
                User test;
                expected.TryGetValue(a.UserName, out test);
                AssertUserAreEqual(test, a);
        
                matchCount++;
            }
        
            Assert.That(matchCount, Is.EqualTo(expected.Count));
        
        
        }
        
        private static void AssertUserAreEqual(User expected, User actual)
        {
            Assert.IsNotNull(actual);
            Assert.That(actual.IsAdmin, Is.EqualTo(expected.IsAdmin));
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            Assert.That(actual.Password, Is.EqualTo(expected.Password));
            Assert.That(actual.UserID, Is.EqualTo(expected.UserID));
            Assert.That(actual.FirstName, Is.EqualTo(expected.FirstName));
            Assert.That(actual.LastName, Is.EqualTo(expected.LastName));
        }
        
        private User CreateTestUser(string userName, string password, string firstName, string lastName, bool admin)
        {
            return UserRepo.AddUser(userName, password, firstName, lastName, admin);
        }
    }
}
