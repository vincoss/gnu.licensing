
## Task (TODO)
## Documentation for the API
Create dev documentation for API (Swager)
	https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-3.1
	app.UseSwagger() for the API doc
API should not request for /favicon.ico

add check for the product registration that the sign key exists on specified path, that shall be on statup, or health check
shall not make request to favicon.ico
	[16:19:30 INF] Request starting HTTP/2 GET https://172.31.249.240/favicon.ico
	[16:19:30 INF] Request finished in 3.9112ms 404

## Enable SSL on localhost|help
dotnet dev-certs https --help
dotnet dev-certs https --trust

## Ef Migration
add-migration initial -verbose
remove-migration
add-migration initial -Context EfDbContext

## Htts configuration (open CMD on root of the repository)
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\Shot.Licensing.Api.pfx -p todo-Pass@word1
dotnet dev-certs https --clean
dotnet dev-certs https --trust
dotnet user-secrets -p src\Shot.Licensing.Api\Shot.Licensing.Api.csproj set "Kestrel:Certificates:Password" "todo-Pass@word1"

## Publish
https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

## Resources

Icon
https://www.compart.com/en/unicode/U+1F765
https://onlineunicodetools.com/convert-unicode-to-image

https://letsencrypt.org/
https://cloud.google.com/run/
