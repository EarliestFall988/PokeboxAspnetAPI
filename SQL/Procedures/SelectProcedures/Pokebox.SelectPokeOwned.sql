-- Get all owned pokemon

CREATE OR ALTER PROCEDURE Pokebox.SelectPokeOwned
AS

SELECT *
FROM Pokebox.PokeOwned
GO
