## Resources
https://hub.docker.com/_/microsoft-dotnet-core-sdk/
https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
https://docs.microsoft.com/en-us/virtualization/windowscontainers/manage-docker/manage-windows-dockerfile
https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows
https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1
https://github.com/dotnet/dotnet-docker/blob/master/samples/run-aspnetcore-https-development.md
https://github.com/dotnet/dotnet-docker/blob/master/samples/host-aspnetcore-https.md
https://www.vivienfabing.com/docker/2019/10/03/docker-aspnetcore-container-and-https.html

NOTE:
The samples are written for cmd.exe.

## Build
docker build --no-cache -t vincoss/shotlicapisvr:1.0.0 .

## Tag image (before publish to docker hub)
docker image tag vincoss/shotlicapisvr:1.0.0 vincoss/shotlicapisvr:1.0.0-windows-nanoserver

## Push to docker hub
docker image push vincoss/shotlicapisvr:1.0.0-windows-nanoserver

## Run
docker run -it --rm -p 8001:443 --name shotlicapisvr -h shotapi --ip 10.1.2.3 -v shotLicData:C:/Shot-Licensing/Data vincoss/shotlicapisvr:1.0.0-windows-nanoserver

## Error logs
docker logs --tail 50 --follow --timestamps shotlicapisvr

## Show running container IP
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" shotlicapisvr

## Browse
https://shotapi/api/license
https://localhost/api/license

##------------------------------------------------ Test

# grab image
docker pull vincoss/shotlicapisvr:1.0.0-windows-nanoserver

# run (with developer certificate)
docker run -it --rm -p 443:443 --name shotlicapisvr -h shotapi --ip 10.1.2.3 --name shotlicapisvr -h shotapi --ip 10.1.2.3 -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="todo-Pass@word1" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Shot.Licensing.Api.pfx -v "%USERPROFILE%/.aspnet/https:C:/https/" -v shotLicData:C:/Shot-Licensing/Data vincoss/shotlicapisvr:1.0.0-windows-nanoserver

#Browse
https://shotapi/api/license
