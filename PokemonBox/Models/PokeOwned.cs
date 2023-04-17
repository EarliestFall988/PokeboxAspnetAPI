namespace PokemonBox.Models
{
    public class PokeOwned
    {
        public uint PokeOwnedID { get; }

        public uint UserID { get; }

        public uint PokemonID { get; }

        public string NickName { get; }

        public DateTimeOffset DatePutInBox { get; }

        public pokeGender Gender { get; }

        public uint Level { get; }

        public PokeOwned( uint pokeOwnedID, uint userID, uint pokemonID, string nickname,
            DateTimeOffset datePutInBox, pokeGender gender, uint level)
        {
            PokeOwnedID = pokeOwnedID;
            UserID = userID;
            PokemonID = pokemonID;
            DatePutInBox = datePutInBox;
            Gender = gender;
            Level = level;
            NickName = nickname;
        }
    }
}
