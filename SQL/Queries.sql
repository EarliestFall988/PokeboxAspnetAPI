EXEC Pokebox.AddPokemon N'Emma', 1, 0, 'Dog shit'

EXEC Pokebox.AddPokemon N'Taylor', 2, 1, 'Literally God'

EXEC Pokebox.AddUser N'a', N'a', N'a', N'a', 1

EXEC Pokebox.AddPokeOwned N'a', N'Taylor', N'Quirky White Bitch', 'N', 69

EXEC Pokebox.AddPokeOwned N'a', N'Emma', N'Quirkiest White Bitch', 'N', 69

EXEC Pokebox.AddPokemonType N'Fire'

EXEC Pokebox.AddPokeType N'Fire', N'Taylor'

EXEC Pokebox.AddItemType N'Pokeball'

EXEC Pokebox.AddItem N'Pokeball', N'Safari Ball', N'It catches things'

EXEC Pokebox.AddItemOwned N'a', N'Safari Ball'

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

EXEC Pokebox.SelectAllPokemonOwnedByUser N'a'

EXEC Pokebox.SelectAllItemsOwnedByUser N'a'