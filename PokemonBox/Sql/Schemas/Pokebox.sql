IF NOT EXISTS
   (
      SELECT *
      FROM sys.schemas s
      WHERE s.[name] = N'Pokebox'
   )
BEGIN
   EXEC(N'CREATE SCHEMA [Pokebox] AUTHORIZATION [dbo]');
END;