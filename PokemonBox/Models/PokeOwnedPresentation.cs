namespace PokemonBox.Models
{
    public class PokeOwnedPresentation
    {
        public uint PokeOwnedID { get; }

        public uint UserID { get; }

        public string PokemonName { get; }

        public string PokemonTypeNameOne { get; }

        public string PokemonTypeNameTwo { get; }

        public uint PokemonID { get; }

        public string NickName { get; }

        public bool IsLegendary { get; }

        public DateTimeOffset DatePutInBox { get; }

        public pokeGender Gender { get; }

        public uint Level { get; }

        public string ImageLink { get; }

        public string Username { get; }

        public PokeOwnedPresentation( string nickname, uint level, pokeGender gender, string pokemonName, string pokemonTypeOne, string pokemonTypeTwo, bool isLegendary, DateTimeOffset datePutInBox,
            string username, uint pokemonID, uint pokeOwnedID, string imageLink)
        {
            PokeOwnedID = pokeOwnedID;
            PokemonID = pokemonID;
            DatePutInBox = datePutInBox;
            Gender = gender;
            Level = level;
            NickName = nickname;
            ImageLink = imageLink;
            IsLegendary = isLegendary;
            PokemonName = pokemonName;
            PokemonTypeNameOne = pokemonTypeOne;
            PokemonTypeNameTwo = pokemonTypeTwo;
            Username = username;
        }
    }
}
