## Resources
https://hub.docker.com/_/microsoft-dotnet-core-sdk/
https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
https://docs.microsoft.com/en-us/virtualization/windowscontainers/manage-docker/manage-windows-dockerfile
https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows
https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1
https://github.com/dotnet/dotnet-docker/blob/master/samples/run-aspnetcore-https-development.md
https://github.com/dotnet/dotnet-docker/blob/master/samples/host-aspnetcore-https.md
https://www.vivienfabing.com/docker/2019/10/03/docker-aspnetcore-container-and-https.html
https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1
https://hub.docker.com/_/microsoft-dotnet-core-aspnet/

NOTE:
The samples are written for cmd.exe.

## Cmd
cd to solution root

## Build & tag
docker build --no-cache -t vincoss/shotlicapisvr:1.0.0-windows .
docker build -f src/Shot.Licensing.DevResources/Docker/Dockerfile.ubuntu-x64 --no-cache -t vincoss/shotlicapisvr:1.0.0-bionic .

## Tag image (before publish to docker hub) if not done yet
docker image tag vincoss/shotlicapisvr:1.0.0 vincoss/shotlicapisvr:1.0.0-windows

## Push to docker hub
docker image push vincoss/shotlicapisvr:1.0.0-windows
docker image push vincoss/shotlicapisvr:1.0.0-bionic

## Run
docker run -it --rm -p 8001:443 --name shotlicapisvr -v shotLicData:C:/Shot.Licensing/Data vincoss/shotlicapisvr:1.0.0-windows
docker run -it --rm -p 8001:443 --name shotlicapisvr -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="p@ssword" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Shot.Licensing.Api.pfx -v "%USERPROFILE%/.aspnet/https:C:/https/" -v shotLicData:C:/Shot.Licensing/Data vincoss/shotlicapisvr:1.0.0-windows

## Run Windows using Linux contaners
docker run -it --rm -p 8002:443 --name shotlicapisvr -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="p@ssword" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Shot.Licensing.Api.pfx -v "%USERPROFILE%\.aspnet\https:/https/" -v c:/temp/shot-licensing:/var/Shot.Licensing/Data vincoss/shotlicapisvr:1.0.0-bionic

docker run -it --rm -p 8001:443 --name shotlicapisvr vincoss/shotlicapisvr:1.0.0-bionic

## Error logs
docker logs --tail 50 --follow --timestamps shotlicapisvr

## Show running container IP
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" shotlicapisvr

## Create developer HTTPS certificate
dotnet dev-certs https -ep "%USERPROFILE%\.aspnet\https\Shot.Licensing.Api.pfx" -p p@ssword
dotnet dev-certs https --trust

##------------------------------------------------ Test

## Browse
https://localhost/api/license
https://localhost:8002/api/license
https://172.17.0.2:8002/api/license
https://shotapi/api/license
https://localhost:8001/api/license
https://{ip-here}/api/license


docker pull mcr.microsoft.com/dotnet/core/samples:aspnetapp
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="password" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%\.aspnet\https:/https/ mcr.microsoft.com/dotnet/core/samples:aspnetapp