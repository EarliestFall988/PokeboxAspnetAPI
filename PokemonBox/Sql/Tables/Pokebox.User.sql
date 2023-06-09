CREATE TABLE Pokebox.[User]
(
    UserID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(64) NOT NULL,
    [Password] NVARCHAR(512) NOT NULL,
    FirstName NVARCHAR(32) NOT NULL,
    LastName NVARCHAR(32) NOT NULL,
    IsAdmin INT NOT NULL DEFAULT(0),
    DateCreated DATETIMEOFFSET NOT NULL DEFAULT(SYSDATETIMEOFFSET())

    UNIQUE(UserName)
)