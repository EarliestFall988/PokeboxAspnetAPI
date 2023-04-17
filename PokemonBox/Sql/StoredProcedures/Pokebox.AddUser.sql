CREATE OR ALTER PROCEDURE Pokebox.AddUser
    @Username NVARCHAR(64),
    @Password NVARCHAR(32),
    @FirstName NVARCHAR(32),
    @LastName NVARCHAR(32),
    @IsAdmin INT,
    @UserID INT OUTPUT
AS
INSERT Pokebox.[User](Username, [Password], [FirstName], LastName, IsAdmin)
VALUES (@Username, @Password, @FirstName, @LastName, @IsAdmin);

SET @UserID = SCOPE_IDENTITY();
GO