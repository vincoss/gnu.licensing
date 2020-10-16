

## Tasks
review the database objects, fix warnings
localize string must verify the code keys and keys in the resources

## Enable SSL on localhost|help
dotnet dev-certs https --help
dotnet dev-certs https --trust

## Htts configuration (open CMD on root of the repository)
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\GnuLicensing.pfx -p todo-Pass@word1
dotnet dev-certs https --clean
dotnet dev-certs https --trust
dotnet user-secrets -p src\Gnu.Licensing.Api\Gnu.Licensing.Api.csproj set "Kestrel:Certificates:Password" "todo-Pass@word1"