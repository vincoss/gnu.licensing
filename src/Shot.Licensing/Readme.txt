
## Generic
+build public key into the app already
find additional documentation
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
shall be able transfer licence from device to device 
(floating licence)
always check for licence when there is internet connectivity. no connectivity and has existing then ok
shall be able to export licence or, license request
user must be able to use registration email that was sent with licence to add new device
possible send email and receive licence again
see teamcity licence for the UI
device_id can be used with apikey as a custom attribute, line rhinoLicensing user request licence then we send it to them
test how to migrate licence between devices, here do some research, must be very easy,
	just ping server with license key, and device id, see rhino licencing
	user can copy licence to many devices but when online it will deatvicate licence for other devices

1. check lic on machine
2. check on server
	if has connection
		if not valid remove or switch to demo 
3. note serve might have only one active licence

## Lic server
licence API
SqlIte database, stand alone app
generate licence|deactivate|activate
licence database
generate license from request
check license
move license to another device
need some ui to show info
private key store somewhere
Always HTTPS
container, configure that, with ssl
Licencing API
	Get server licence
	Get app licence
lincense UI to view|generate licence|deactivate|activate
licence server API should be stand alose server with Sqlite database to store licences for all products


## Resources
https://github.com/junian/Standard.Licensing
https://github.com/dnauck/Portable.Licensing
https://www.nuget.org/packages/Standard.Licensing
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-sign-xml-documents-with-digital-signatures
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-verify-the-digital-signatures-of-xml-documents




	

