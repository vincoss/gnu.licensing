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
docker build -f src/Gnu.Licensing.DevResources/Docker/Dockerfile.win-x64 --no-cache -t vincoss/gnulicensesvr:1.0.0-windows .
docker build -f src/Gnu.Licensing.DevResources/Docker/Dockerfile.ubuntu-x64 --no-cache -t vincoss/gnulicensesvr:1.0.0-bionic .
docker build -f src/Gnu.Licensing.DevResources/Docker/Dockerfile.ubuntu-arm --no-cache -t vincoss/gnulicensesvr:1.0.0-bionic-arm .

## Tag image (before publish to docker hub) if not done yet
docker image tag vincoss/gnulicensesvr:1.0.0 vincoss/gnulicensesvr:1.0.0-windows

## Push to docker hub
docker image push vincoss/gnulicensesvr:1.0.0-windows
docker image push vincoss/gnulicensesvr:1.0.0-bionic
docker image push vincoss/gnulicensesvr:1.0.0-bionic-arm

## Run
docker run -it --rm -p 1999:443 --name gnulicensesvr -v c:/var/appdata:c:/var/appdata vincoss/gnulicensesvr:1.0.0-windows
docker run -it --rm -p 1999:443 --name gnulicensesvr -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=1999 -e ASPNETCORE_Kestrel__Certificates__Default__Password="Pass@word1" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Gnu.Licensing.pfx -v "%USERPROFILE%/.aspnet/https:C:/https/" -v c:/var/appdata:c:/var/appdata vincoss/gnulicensesvr:1.0.0-windows

## Run Windows using Linux contaners
docker run -it --rm -p 1999:443 --name gnulicensesvr -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=1999 -e ASPNETCORE_Kestrel__Certificates__Default__Password="Pass@word1" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Gnu.Licensing.pfx -v "%USERPROFILE%\.aspnet\https:/https/" -v c:/var/appdata:/var/appdata vincoss/gnulicensesvr:1.0.0-bionic
docker run -it --rm -p 1999:443 --name gnulicensesvr -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORT=1999 -e ASPNETCORE_Kestrel__Certificates__Default__Password="Pass@word1" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Gnu.Licensing.pfx -v "%USERPROFILE%\.aspnet\https:/https/" -v c:/var/appdata:/var/appdata vincoss/gnulicensesvr:1.0.0-bionic-arm

## Error logs
docker logs --tail 50 --follow --timestamps gnulicensesvr

## Show running container IP
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" gnulicensesvr
docker exec -it gnulicensesvr bash

## Create developer HTTPS certificate
dotnet dev-certs https -ep "%USERPROFILE%\.aspnet\https\Gnu.Licensing.pfx" -p Pass@word1
dotnet dev-certs https --trust

## Switch
-d detaches the process and runs it in the background.

##------------------------------------------------ Test

## Browse
https://localhost/api/license
https://localhost:1999/api/license
https://172.17.0.2:1999/api/license
https://shotapi/api/license
https://{ip-here}/api/license


docker pull mcr.microsoft.com/dotnet/core/samples:aspnetapp
docker run --rm -it -p 8000:80 -p 1999:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=1999 -e ASPNETCORE_Kestrel__Certificates__Default__Password="password" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%\.aspnet\https:/https/ mcr.microsoft.com/dotnet/core/samples:aspnetapp