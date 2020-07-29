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
docker build -f src/Gnu.Licensing.DevResources/Docker/Dockerfile --no-cache -t vincoss/gnulicapisvr:1.0.0-windows .
docker build -f src/Gnu.Licensing.DevResources/Docker/Dockerfile.ubuntu-x64 --no-cache -t vincoss/gnulicapisvr:1.0.0-bionic .
docker build -f src/Gnu.Licensing.DevResources/Docker/Dockerfile.ubuntu-arm --no-cache -t vincoss/gnulicapisvr:1.0.0-bionic-arm .

## Tag image (before publish to docker hub) if not done yet
docker image tag vincoss/gnulicapisvr:1.0.0 vincoss/gnulicapisvr:1.0.0-windows

## Push to docker hub
docker image push vincoss/gnulicapisvr:1.0.0-windows
docker image push vincoss/gnulicapisvr:1.0.0-bionic
docker image push vincoss/gnulicapisvr:1.0.0-bionic-arm

## Run
docker run -it --rm -p 8001:443 --name gnulicapisvr -v shotLicData:C:/Gnu.Licensing/Data vincoss/gnulicapisvr:1.0.0-windows
docker run -it --rm -p 8002:443 --name gnulicapisvr -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=8002 -e ASPNETCORE_Kestrel__Certificates__Default__Password="Pass@word1" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Gnu.Licensing.pfx -v "%USERPROFILE%/.aspnet/https:C:/https/" -v c:/temp/gnu-licensing:c:/var/appdata vincoss/gnulicapisvr:1.0.0-windows

## Run Windows using Linux contaners
docker run -it --rm -p 8002:443 --name gnulicapisvr -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=8002 -e ASPNETCORE_Kestrel__Certificates__Default__Password="Pass@word1" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Gnu.Licensing.pfx -v "%USERPROFILE%\.aspnet\https:/https/" -v c:/temp/gnu-licensing:/var/appdata vincoss/gnulicapisvr:1.0.0-bionic

## Error logs
docker logs --tail 50 --follow --timestamps gnulicapisvr

## Show running container IP
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" gnulicapisvr
docker exec -it gnulicapisvr bash

## Create developer HTTPS certificate
dotnet dev-certs https -ep "%USERPROFILE%\.aspnet\https\Gnu.Licensing.pfx" -p Pass@word1
dotnet dev-certs https --trust

## Switch
-d detaches the process and runs it in the background.

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