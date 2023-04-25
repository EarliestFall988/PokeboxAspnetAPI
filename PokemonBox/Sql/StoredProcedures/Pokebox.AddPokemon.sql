CREATE OR ALTER PROCEDURE Pokebox.AddPokemon
    @PokemonName NVARCHAR(32),
    @PokedexNumber INT,
    @IsLegendary INT,
    @ImageLink NVARCHAR(256),
    @PokemonID INT OUTPUT,
    @DateAdded DATETIMEOFFSET OUTPUT
AS
INSERT Pokebox.Pokemon(PokemonName, PokedexNumber, IsLegendary, ImageLink)
VALUES (@PokemonName, @PokedexNumber, @IsLegendary, @ImageLink);

SET @PokemonID = SCOPE_IDENTITY();
SET @DateAdded = SYSDATETIMEOFFSET();
GO