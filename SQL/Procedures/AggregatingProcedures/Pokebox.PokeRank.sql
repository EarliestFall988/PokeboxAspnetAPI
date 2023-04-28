-- Rank users by the number of a specific pokemon they have

CREATE OR ALTER PROCEDURE Pokebox.PokeRank
    @PokemonName NVARCHAR(64)
AS

SELECT U.UserID, U.Username, SUM(IIF(P.PokemonName = @PokemonName, 1, 0)) AS PokemonCount, 
    RANK() OVER (ORDER BY SUM(IIF(P.PokemonName = @PokemonName, 1, 0))DESC) AS Rank
FROM Pokebox.Pokemon P
    INNER JOIN Pokebox.PokeOwned PO ON PO.PokemonID = P.PokemonID
    RIGHT JOIN Pokebox.[User] U ON U.UserID = PO.UserID
GROUP BY U.UserID, U.Username
ORDER BY Rank