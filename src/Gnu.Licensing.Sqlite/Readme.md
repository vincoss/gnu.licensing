# Gnu.Licensing SQLite Database Provider

This project contains Gnu.Licensing SQLite database provider.

## Migrations

Add a migration with:

```
dotnet ef migrations add initial --context SqliteContext --output-dir Migrations --startup-project ..\Gnu.Licensing.Api\Gnu.Licensing.Api.csproj

dotnet ef database update --context SqliteContext
```
