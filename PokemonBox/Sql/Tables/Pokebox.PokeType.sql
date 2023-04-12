CREATE TABLE Pokebox.PokeType
(
    PokemonID INT NOT NULL FOREIGN KEY
        REFERENCES Pokebox.Pokemon(PokemonID),
    PokemonTypeID INT NOT NULL FOREIGN KEY
        REFERENCES Pokebox.PokemonType(PokemonTypeID),

    PRIMARY KEY(PokemonID, PokemonTypeID)
)