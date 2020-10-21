

## Tasks
search for TODO
review the database objects, fix warnings
localize string must verify the code keys and keys in the resources
allow 5 license activation per on purchased license, possible allow configuration that on the product
register license send more info about device (easier to manage|unregister the license)
	see the device info sample
	if show attributes then user can see AppId on device then know what device use particular activation
TODO: public int MaxActivations { get; set; } = 5;
if user get 2 licenses the must have license registration for each???

## Enable SSL on localhost|help
dotnet dev-certs https --help
dotnet dev-certs https --trust

## Htts configuration (open CMD on root of the repository)
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\GnuLicensing.pfx -p todo-Pass@word1
dotnet dev-certs https --clean
dotnet dev-certs https --trust
dotnet user-secrets -p src\Gnu.Licensing.Api\Gnu.Licensing.Api.csproj set "Kestrel:Certificates:Password" "todo-Pass@word1"
