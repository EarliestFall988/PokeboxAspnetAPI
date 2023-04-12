CREATE OR ALTER PROCEDURE Pokebox.AddPokemon
    @PokemonName NVARCHAR(32),
    @PokedexNumber INT,
    @IsLegendary INT,
    @Description NVARCHAR(128)
AS
INSERT Pokebox.Pokemon(PokemonName, PokedexNumber, IsLegendary, [Description])
VALUES (@PokemonName, @PokedexNumber, @IsLegendary, @Description);
GO