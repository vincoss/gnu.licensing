
## Generic
+create cli project
+find additional documentation (at home)
+generate public and private key

## Database
+license registration, LicenseUuid and ProductUuid must be unique

## Client App
+build public key into the app already
+license on client store in the root directory or secureStore
+	root directory is good (there is some reason for that)
+	secure store if user install and uninstall app no need to get new license
+license.xml name as
+shall be able transfer licence from device to device, user can use license ID to get new one from server. Its personal licese so for now user can use on many devices.
+user must be able to use registration email that was sent with licence to add new device
+device_id can be used as AppId as a custom attribute
+display license information

## Localization
+add translations for the error messages

## Samples
+Create sample Warthog.Sample_Console
+Create sample Warthog.Sample_Console_ServerLicenseFetch
+Create sample Warthog.Sample_Xamarin

## Server
see balsamique license request email if user needs to re-fetch license email. send same email as for registration.
possible use license API to retrieve an license again by email and sent an email then, use original request data
add the the licensing dll from nuget after publish, cli too
remove the registered licenses
	return UI page with list of devices to easy manage, must enter email, and license ID
	problem is that user can get email from license, must sent the page to the user email address
	or send access token then user can access management page
+health check
add recover license url on help section
	must emapil license like Balsamique
add license server urls

## Server UI
see teamcity licence for the UI
+all dates to UTC
+License Keys Black-Listing
+	Any license key can be blacklisted, which means it won't be accepted by  even if it is valid.
+	Registration IsValid to false
+licence API
+SqlIte database, stand alone app
generate licence|deactivate|activate|
+licence database
+generate license from request
check license
need some ui to show info
+Always HTTPS
+licence server API should be stand alose server with Sqlite database to store licences for all products
docker container, configure that, with ssl
Licencing API
	Get server licence
	Get app licence
lincense UI to view|generate licence|deactivate|activate
keys store multiple by name and private|public key
	name is for the app or key store,
	lic must store used name for the reference, what is this about?
	encrypt keys
	private key store somewhere

## Check Lic on App tasks (APP specific)
always check for licence when there is internet connectivity. no connectivity and has existing then ok
	at least one time a day
if not valid remove the license.xml file, this only if http check was successful
if https check fail do nothing if license.xml exists not to be removed
license file might not be valid possible to remove it if tampered
overused licensing ACT05Code
	note each check can write info about the app license then increment something and can see whether license is used on many devices over time
	can collect on device some info each time app starts then send to the server
		last time started
		use counters
	then from those information we can determined wheter uses the APP on multiple devices same time and flag and overused or make it inactive
test how to migrate licence between devices, here do some research, must be very easy,
	just ping server with license key, and additional attributes, include license hash if exists
	user can copy licence to many devices but when online it will deatvicate licence for other devices
test install uninstall, how it affect, lic, pwd and other

## Nice to have
license server & client timeout setting, setting for httpClient timeout, default is ok


## Resources
https://github.com/junian/standard.Licensing
https://github.com/dnauck/Portable.Licensing
https://www.nuget.org/packages/Warthog
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-sign-xml-documents-with-digital-signatures
https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-verify-the-digital-signatures-of-xml-documents

