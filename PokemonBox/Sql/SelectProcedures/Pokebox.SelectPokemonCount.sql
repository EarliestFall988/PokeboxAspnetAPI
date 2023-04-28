CREATE OR ALTER PROCEDURE Pokebox.SelectPokemonCount
AS

SELECT COUNT(*) AS PokemonCount
FROM Pokebox.Pokemon
GO
