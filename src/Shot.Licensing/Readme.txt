
## Generic
+create cli project
+find additional documentation (at home)
+generate public and private key
export licence
import licence
verify licence

## Database
license registration, LicenseUuid and ProductUuid must be unique

## Client
+build public key into the app already
license on client store in the root directory or secureStore
	root directory is good (there is some reason for that)
	secure store if user install and uninstall app no need to get new license
License.xml (name as)
test install uninstall, how it affect, lic, pwd and other
shall be able transfer licence from device to device, user can use license ID to get new one from server.
always check for licence when there is internet connectivity. no connectivity and has existing then ok
	at least one time a day
user must be able to use registration email that was sent with licence to add new device
device_id can be used with apikey as a custom attribute, line rhinoLicensing user request licence then we send it to them
display license information
http request client timeout setting
if not valid remove the license.xml file
if http check fail do nothing if license.xml exists not to be removed
license file might not be valid possible to remove it if tampered
add translations

## Testing
test how to migrate licence between devices, here do some research, must be very easy,
	just ping server with license key, and device id, see rhino licencing
	user can copy licence to many devices but when online it will deatvicate licence for other devices

## Samples
Create sample Shot.Licensing.Sample_Console
Create sample Shot.Licensing.Sample_Console_ServerLicenseFetch
Create sample Shot.Licensing.Sample_Wpf
Create sample Shot.Licensing.Sample_Website
Create sample Shot.Licensing.Sample_Xamarin

## Server
ACT00Code, ACT01Code not used
lincese server & client timeout setting, setting for httpClient timeout
see balsamique license request email if user needs to re-fetch license email. send same email as for registration.
Licencing use for API key that will have pairs what can sync get|post. The APi key is from licence ID
possible use license API to retrieve an license again by email and sent an email then, use original request data
add the the licensing dll from nuget after publish, cli too

## Server UI
see teamcity licence for the UI
all dates to UTC
License Keys Black-Listing
	Any license key can be blacklisted, which means it won't be accepted by  even if it is valid.
licence API
SqlIte database, stand alone app
generate licence|deactivate|activate|
licence database
generate license from request
check license
need some ui to show info
private key store somewhere
Always HTTPS
container, configure that, with ssl
Licencing API
	Get server licence
	Get app licence
lincense UI to view|generate licence|deactivate|activate
licence server API should be stand alose server with Sqlite database to store licences for all products
keys store multiple by name and private|public key
	name is for the app or key store,
	lic must store used name for the reference

## Temp
1. check lic on machine
	exists no demo
	yes valid no demo
	yes vallid full
2. check on server
	if has connection
		if not valid remove or switch to demo 
3. note server might have only one active licence if not a volume license

if quanity = 1
	only one can be active at a time
if quanity > 1
	multiple can be active at a time

CreateDateTimeUtc

Not used for volume license) device ID not to be used

Hi With the volume license, there is no check on the uid of the machine. that means it can be activated infinitely many machines.

http://ellipter.com/contact/
https://www.codeproject.com/Articles/996001/A-Ready-To-Use-Software-Licensing-Solution-in-Csha?fid=1897432&df=90&mpp=25&sort=Position&spc=Relaxed&prof=True&view=Normal&fr=76#xx0xx


## Localization
need to localize build in messages InvalidSignatureValidationFailure


## Example Fiddler compose to register license

POST https://localhost:5001/api/license HTTP/1.1
Host: localhost:5001
Content-Type: application/json

{"LicenseId":"D4248D45-7B4A-4832-A7D1-6AA32A752453","Attributes":{"ClientId":"FAAAEB70-3BCF-4FDC-B67A-5C6B81C316C5"}}

## Resources
https://github.com/junian/Shot.Licensing
https://github.com/dnauck/Portable.Licensing
https://www.nuget.org/packages/Shot.Licensing
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-sign-xml-documents-with-digital-signatures
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-verify-the-digital-signatures-of-xml-documents




	

