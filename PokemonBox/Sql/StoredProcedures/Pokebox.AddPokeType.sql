CREATE OR ALTER PROCEDURE Pokebox.AddPokeType
    @PokemonTypeName NVARCHAR(16),
    @PokemonName NVARCHAR(64)
AS

DECLARE @PokemonTypeID INT =
    (
        SELECT P.PokemonTypeID
        FROM Pokebox.PokemonType P
        WHERE P.PokemonTypeName = @PokemonTypeName
    )

DECLARE @PokemonID INT = 
    (
        SELECT P.PokemonID
        FROM Pokebox.Pokemon P
        WHERE P.PokemonName = @PokemonName
    )

INSERT Pokebox.PokeType(PokemonTypeID, PokemonID)
VALUES(@PokemonTypeID, @PokemonID);
GO
