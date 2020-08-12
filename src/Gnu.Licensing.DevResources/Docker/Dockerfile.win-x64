# base image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

ARG HTTPS_PORT=443
ARG APP_HOME=/var/app

WORKDIR ${APP_HOME}
COPY ["/src/Gnu.Licensing.Svr/bin/Release/netcoreapp3.1/publish/win-x64/.", "./"]

ENV HOME ${APP_HOME}
ENV NAME shotlicapisvr

# Make ports available to the world outside this container for main web interface
EXPOSE ${HTTPS_PORT}

VOLUME c:/var/appdata

ENTRYPOINT ["dotnet", "Gnu.Licensing.Svr.dll"]