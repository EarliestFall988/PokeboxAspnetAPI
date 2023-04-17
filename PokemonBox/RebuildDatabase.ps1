Param(
   [string] $Server = "(localdb)\MSSQLLocalDb",
   [string] $Database = "PokemonBoxDatabase"
)

# This script requires the SQL Server module for PowerShell.
# The below commands may be required.

# To check whether the module is installed.
Get-Module -ListAvailable -Name SqlServer;

# Install the SQL Server Module
#Install-Module -Name SqlServer -Scope CurrentUser

$CurrentDrive = (Get-Location).Drive.Name + ":"

Write-Host ""
Write-Host "Rebuilding database $Database on $Server..."

<#
   If on your local machine, you can drop and re-create the database.
#>
& ".\Scripts\DropDatabase.ps1" -Database $Database
& ".\Scripts\CreateDatabase.ps1" -Database $Database

<#
   If on the department's server, you don't have permissions to drop or create databases.
   In this case, maintain a script to drop all tables.
#>
#Write-Host "Dropping tables..."
#Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "PersonData\Sql\Tables\DropTables.sql"

Write-Host "Creating schema..."
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Schemas\Pokebox.sql"

Write-Host "Creating tables..."
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.ItemType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.Item.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.User.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.Pokemon.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.PokemonType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.PokeType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.PokeOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Tables\Pokebox.ItemOwned.sql"

Write-Host "Stored procedures..."
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\AggregatingProcedures\Pokebox.AverageLevel.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\AggregatingProcedures\Pokebox.PokeRank.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\AggregatingProcedures\Pokebox.PokeTypeCount.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\AggregatingProcedures\Pokebox.TopItem.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\RemoveProcedures\Pokebox.RemoveItemOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\RemoveProcedures\Pokebox.RemovePokeOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectAllItemsOwnedByUser.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectAllPokemonOwnedByUser.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectItem.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectItemOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectItemType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectPokemon.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectPokemonType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectPokeType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectPokeOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectSinglePokeOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\SelectProcedures\Pokebox.SelectUser.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddItem.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddItemOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddItemType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddPokemon.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddPokemonOwned.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddPokemonType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddPokeType.sql"
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\StoredProcedures\Pokebox.AddUser.sql"

Write-Host "Inserting data..."
Invoke-SqlCmd -ServerInstance $Server -Database $Database -InputFile "Sql\Data\FillTable.sql"

Write-Host "Rebuild completed."
Write-Host ""

Set-Location $CurrentDrive
