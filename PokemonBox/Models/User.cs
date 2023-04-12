namespace PokemonBox.Models
{
    public class User
    {
        public uint UserID { get; }

        public string UserName { get; }

        public string Password { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public bool IsAdmin { get; }

        public User(uint userID, string userName, string password, string firstName,
            string lastName, bool isAdmin) 
        {
            UserID = userID;
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            IsAdmin = isAdmin;
        }
    }
}
