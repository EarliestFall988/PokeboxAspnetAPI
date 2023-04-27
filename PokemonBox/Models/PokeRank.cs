namespace PokemonBox.Models
{
    public class PokeRank
    {
        public uint UserID { get; }

        public string Username { get; }

        public uint PokemonCount { get; }

        public uint Rank { get; }   

        public PokeRank(uint userID, string username, uint pokemonCount, uint rank)
        {
            UserID = userID;
            Username = username;
            PokemonCount = pokemonCount;
            Rank = rank;
        }
    }
}
