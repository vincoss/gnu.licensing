

## Tasks
review the database objects, fix warnings
shall not return license if not active
if there is internet connection connection but activation fails example HTTPS not valid then shows VAL01 which is not right
localize string must verify the code keys and keys in the resources

## Ef Migration
add-migration initial -verbose
remove-migration
add-migration initial -Context EfDbContext

## Enable SSL on localhost|help
dotnet dev-certs https --help
dotnet dev-certs https --trust

## Htts configuration (open CMD on root of the repository)
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\GnuLicensing.pfx -p todo-Pass@word1
dotnet dev-certs https --clean
dotnet dev-certs https --trust
dotnet user-secrets -p src\Gnu.Licensing.Api\Gnu.Licensing.Api.csproj set "Kestrel:Certificates:Password" "todo-Pass@word1"