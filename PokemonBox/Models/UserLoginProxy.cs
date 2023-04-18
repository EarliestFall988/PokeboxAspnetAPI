namespace PokemonBox.Models
{
    public class UserLoginProxy
    {
        public string Password { get; set; }

        public string Email { get; set; }

        public UserLoginProxy(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
