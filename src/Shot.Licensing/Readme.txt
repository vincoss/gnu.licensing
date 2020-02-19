
## Generic
+build public key into the app already
Create sample Shot.Licensing.Sample_Wpf
Create sample Shot.Licensing.Sample_Website
Create sample Shot.Licensing.Sample_Xamarin
export licence
import licence
verify licence
lincese server timeout, clinet too
license on client store in the root directory or secureStore
App store lic location
	store on root path
	need provider database or file
	store in settings no need to regenerate if uninstall install
	test install uninstall, how it affect, lic, pwd and other

1. check lic on machine
2. check on server
	if has connection
		if not valid remove or switch to demo 
3. note serve might have only one active licence

## Lic server
licence API
SqlIte database, stand alowe app
generate licence|deactivate|activate
licence database
generate license from request
check license
move license to another device
need some ui to show info
private key store somewhere

## Resources
https://github.com/junian/Standard.Licensing
https://github.com/dnauck/Portable.Licensing
https://www.nuget.org/packages/Standard.Licensing
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-sign-xml-documents-with-digital-signatures
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-verify-the-digital-signatures-of-xml-documents