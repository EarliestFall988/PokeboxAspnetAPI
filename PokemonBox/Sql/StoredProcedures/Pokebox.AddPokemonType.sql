CREATE OR ALTER PROCEDURE Pokebox.AddPokemonType
    @TypeName NVARCHAR(32)
AS
INSERT Pokebox.PokemonType(PokemonTypeName)
VALUES (@TypeName);
GO