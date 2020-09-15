

## Ef Migration
add-migration initial -verbose
remove-migration
add-migration initial -Context EfDbContext

## Enable SSL on localhost|help
dotnet dev-certs https --help
dotnet dev-certs https --trust
