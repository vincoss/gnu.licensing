
## Task 

search for TODO
publish to docker, linux, linuxArm, windows
publish to nuget
create documentation docker
create repo documentation
sign assembly
svr (prl) (web) api to create projects
svr (prl) (web) license requests

	
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

## Resources (server UI private source)
https://keygen.sh/pricing/
https://hackernoon.com/0-to-2000-mrr-in-one-month-lessons-learned-9f6d11ad60ae
https://cs.uwaterloo.ca/~echrzano/all.htm#6.6
https://media.3ds.com/support/simulia/public/flexlm108/EndUser/chap7.htm
https://imedea.uib-csic.es/master/cambioglobal/Modulo_V_cod101615/matlab_7_4/win32/help/base/install/pc/f2-46589.html
https://imedea.uib-csic.es/master/cambioglobal/Modulo_V_cod101615/matlab_7_4/win32/help/base/install/pc/f2-46589.html
https://support.eset.com/en/kb6996-resolve-act-or-ecp-errors-during-activation-business-users
https://support.eset.com/en/kb332-ports-and-addresses-required-to-use-your-eset-product-with-a-third-party-firewall#services
https://stackoverflow.com/questions/36197823/c-how-to-store-and-check-a-generated-license-key



		

	



