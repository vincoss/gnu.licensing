# Gnu.Licensing.SqlServer Database Provider

This project contains Gnu.Licensing SQL Server database provider.

## Migrations

Add a migration with:

```
dotnet ef migrations add initial --context SqlServerContext --output-dir Migrations --startup-project ..\Gnu.Licensing.Api\Gnu.Licensing.Api.csproj

dotnet ef database update --context SqlServerContext
```
