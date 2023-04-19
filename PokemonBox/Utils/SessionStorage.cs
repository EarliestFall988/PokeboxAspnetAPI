namespace PokemonBox.Utils
{
    public static class SessionStorage // this is technically a singleton, I know - please move the data to the db instead
    {

        public static Dictionary<string, string> Sessions = new Dictionary<string, string>();

    }
}
