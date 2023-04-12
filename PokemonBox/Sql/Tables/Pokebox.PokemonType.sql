CREATE TABLE Pokebox.PokemonType
(
    PokemonTypeID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    PokemonTypeName NVARCHAR(16) NOT NULL,

    UNIQUE(PokemonTypeName) 
)
