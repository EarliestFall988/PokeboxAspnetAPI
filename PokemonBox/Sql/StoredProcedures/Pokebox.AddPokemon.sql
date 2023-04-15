CREATE OR ALTER PROCEDURE Pokebox.AddPokemon
    @PokemonName NVARCHAR(32),
    @PokedexNumber INT,
    @IsLegendary INT,
    @Description NVARCHAR(256),
    @PokemonID INT OUTPUT,
    @DateAdded DATETIMEOFFSET OUTPUT
AS
INSERT Pokebox.Pokemon(PokemonName, PokedexNumber, IsLegendary, [Description])
VALUES (@PokemonName, @PokedexNumber, @IsLegendary, @Description);

SET @PokemonID = SCOPE_IDENTITY();
SET @DateAdded = SYSDATETIMEOFFSET();
GO