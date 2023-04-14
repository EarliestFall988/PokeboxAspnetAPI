EXEC Pokebox.AddPokemon N'Emma', 1, 1, 'Dog shit'

EXEC Pokebox.AddPokemon N'Taylor', 2, 1, 'Literally God'

EXEC Pokebox.AddPokemon N'Jacob', 3, 1, 'Literally God'

EXEC Pokebox.AddPokemon N'Micah', 4, 1, 'Literally God'

EXEC Pokebox.AddUser N'a', N'a', N'a', N'a', 1

EXEC Pokebox.AddUser N'b', N'b', N'b', N'b', 0

EXEC Pokebox.AddUser N'c', N'c', N'c', N'c', 0

EXEC Pokebox.AddPokeOwned N'a', N'Taylor', N'Quirky White Bitch', 'N', 69

EXEC Pokebox.AddPokeOwned N'a', N'Emma', N'Quirkiest White Bitch', 'N', 49

EXEC Pokebox.AddPokeOwned N'b', N'Jacob', N'Quirkiest White Bitch', 'N', 49

EXEC Pokebox.AddPokeOwned N'b', N'Micah', N'Quirkiest White Bitch', 'N', 100

EXEC Pokebox.AddPokeOwned N'b', N'Taylor', N'Quirky White Bitch', 'N', 69
EXEC Pokebox.AddPokeOwned N'b', N'Taylor', N'Quirky White', 'N', 69

EXEC Pokebox.AddPokemonType N'Fire'

EXEC Pokebox.AddPokemonType N'Sassy'

EXEC Pokebox.AddPokemonType N'Water'

EXEC Pokebox.AddPokeType N'Sassy', N'Taylor'

EXEC Pokebox.AddPokeType N'Sassy', N'Jacob'

EXEC Pokebox.AddPokeType N'Sassy', N'Micah'

EXEC Pokebox.AddPokeType N'Fire', N'Emma'

EXEC Pokebox.AddItemType N'Pokeball'

EXEC Pokebox.AddItemType N'Potion'

EXEC Pokebox.AddItem N'Pokeball', N'Safari Ball', N'It catches things'

EXEC Pokebox.AddItem N'Pokeball', N'Penis Ball', N'It catches things'

EXEC Pokebox.AddItem N'Potion', N'Health Potion', N'It heals things'

EXEC Pokebox.AddItemOwned N'a', N'Safari Ball'

EXEC Pokebox.AddItemOwned N'b', N'Safari Ball'

EXEC Pokebox.AddItemOwned N'a', N'Penis Ball'

EXEC Pokebox.AddItemOwned N'a', N'Health Potion'

SELECT *
FROM Pokebox.[User]

SELECT *
FROM Pokebox.Pokemon

SELECT *
FROM Pokebox.PokeOwned

SELECT *
FROM Pokebox.PokemonType

SELECT *
FROM Pokebox.PokeType

SELECT *
FROM Pokebox.ItemOwned

SELECT *
FROM Pokebox.ItemType

SELECT *
FROM Pokebox.Item

EXEC Pokebox.SelectAllPokemonOwnedByUser N'et.rutrum@google.net'

EXEC Pokebox.SelectAllItemsOwnedByUser N'et.rutrum@google.net'

EXEC Pokebox.RemoveItemOwned N'a', N'Health Potion'

EXEC Pokebox.SelectAllItemsOwnedByUser N'a'

EXEC Pokebox.TopItem 4, 2023

EXEC Pokebox.PokeTypeCount '2023-04-01', '2023-04-30'

EXEC Pokebox.AverageLevel

EXEC Pokebox.PokeRank N'Numel'