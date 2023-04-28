-- Add a new pokemon type

CREATE OR ALTER PROCEDURE Pokebox.AddPokemonType
    @TypeName NVARCHAR(32),
    @PokemonTypeID INT OUTPUT
AS
INSERT Pokebox.PokemonType(PokemonTypeName)
VALUES (@TypeName);

SET @PokemonTypeID = SCOPE_IDENTITY();
GO