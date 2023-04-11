CREATE OR ALTER PROCEDURE Pokebox.AddPokemonType
    @TypeName NVARCHAR(32)
AS
INSERT Pokebox.PokemonType(PokemonTypeName)
VALUES (@TypeName);

GO

------------------------------------------------------

CREATE OR ALTER PROCEDURE Pokebox.AddItemType
    @TypeName NVARCHAR(32)
AS
INSERT Pokebox.ItemType(ItemTypeName)
VALUES (@TypeName);

GO

----------------------------------------------------------
    
CREATE OR ALTER PROCEDURE Pokebox.AddItem
    @ItemTypeName NVARCHAR(64),
    @ItemName NVARCHAR(64),
    @Description NVARCHAR(128)
AS
INSERT Pokebox.Item(ItemTypeID, ItemName, [Description])
SELECT IT.ItemTypeID, @ItemName, @Description
FROM ItemType IT 
WHERE IT.ItemTypeName = @ItemTypeName;

----------------------------------------------------------
GO

CREATE OR ALTER PROCEDURE Pokebox.AddUser
    @Username NVARCHAR(32),
    @Password NVARCHAR(32),
    @FirstName NVARCHAR(32),
    @LastName NVARCHAR(32),
    @IsAdmin INT
AS
INSERT Pokebox.[User](Username, [Password], [FirstName], LastName, IsAdmin)
VALUES (@Username, @Password, @FirstName, @LastName, @IsAdmin);

-----------------------------------------------------------

GO

CREATE OR ALTER PROCEDURE Pokebox.AddPokemon
    @PokemonName NVARCHAR(32),
    @PokedexNumber INT,
    @IsLegendary INT,
    @Description NVARCHAR(128)
AS
INSERT Pokebox.Pokemon(PokemonName, PokedexNumber, IsLegendary, [Description])
VALUES (@PokemonName, @PokedexNumber, @IsLegendary, @Description);

-----------------------------------------------------------------

GO

CREATE OR ALTER PROCEDURE Pokebox.AddPokeOwned
    @Username NVARCHAR(32),
    @PokemonName NVARCHAR(64),
    @Name NVARCHAR(64),
    @Gender NVARCHAR(1),
    @Level INT
AS

DECLARE @UserID INT =
    (
        SELECT U.UserID
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @PokemonID INT = 
    (
        SELECT P.PokemonID
        FROM Pokebox.Pokemon P
        WHERE P.PokemonName = @PokemonName
    )

INSERT Pokebox.PokeOwned(UserID, PokemonID, [Name], Gender, [Level])
VALUES(@UserID, @PokemonID, @Name, @Gender, @Level);


----------------------------------------------------------------------

GO

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

------------------------------------------------------------------------

GO
CREATE OR ALTER PROCEDURE Pokebox.AddItemOwned
    @Username NVARCHAR(32),
    @ItemName NVARCHAR(64)
AS

DECLARE @UserID INT =
    (
        SELECT U.UserID
        FROM Pokebox.[User] U
        WHERE U.Username = @Username
    )

DECLARE @ItemID INT = 
    (
        SELECT I.ItemID
        FROM Pokebox.Item I
        WHERE I.ItemName = @ItemName
    )

INSERT Pokebox.ItemOwned(UserID, ItemID)
VALUES(@UserID, @ItemID);

------------------------------------------------------------------------











