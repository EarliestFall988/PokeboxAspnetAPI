CREATE OR ALTER PROCEDURE Pokebox.SelectPokemonType
AS

SELECT *
FROM Pokebox.PokemonType

---------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.SelectPokeType
AS

SELECT *
FROM Pokebox.PokeType

---------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.SelectPokemon
AS

SELECT *
FROM Pokebox.Pokemon

---------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.SelectItemOwned
AS

SELECT *
FROM Pokebox.ItemOwned

---------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.SelectItem
AS

SELECT *
FROM Pokebox.Item

---------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.SelectItemType
AS

SELECT *
FROM Pokebox.ItemType

----------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.SelectAllPokemonOwnedByUser
    @Username NVARCHAR(32)
AS

SELECT PO.Name, PO.[Level], PO.Gender, P.PokemonName, POT.PokemonTypeName, PO.DatePutInBox
FROM [User] U
    INNER JOIN PokeOwned PO ON PO.UserID = U.UserID
    INNER JOIN Pokemon P ON P.PokemonID = PO.PokemonID
    INNER JOIN PokeType PT ON PT.PokemonID = P.PokemonID
    INNER JOIN PokemonType POT ON POT.PokemonTypeID = PT.PokemonTypeID 
WHERE U.Username = @Username

-----------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.SelectAllItemsOwnedByUser
    @Username NVARCHAR(32)
AS

SELECT I.ItemName, IT.ItemTypeName, IOW.DatePutInBox
FROM [User] U 
    INNER JOIN ItemOwned IOW ON IOW.UserID = U.UserID
    INNER JOIN Item I ON I.ItemID = IOW.ItemID
    INNER JOIN ItemType IT ON IT.ItemTypeID = I.ItemTypeID