CREATE TABLE Pokebox.Pokemon
(
    PokemonID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    PokemonName NVARCHAR(32) NOT NULL,
    PokedexNumber INT NOT NULL,
    IsLegendary INT NOT NULL DEFAULT(0),
    [Description] NVARCHAR(128) NOT NULL,
    DateAdded DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET()),
    ImageLink NVARCHAR(256) NULL DEFAULT(N''),

    UNIQUE(PokemonName),

    UNIQUE(PokedexNumber)

)