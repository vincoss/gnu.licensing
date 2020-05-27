# base image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS runtime

ARG HTTPS_PORT=44395
ARG SHOT_LICENSING_API_HOME=/inetpub/wwwroot/shot.licensing.api

WORKDIR ${SHOT_LICENSING_API_HOME}
COPY ["/src/Shot.Licensing.Api/bin/Release/netcoreapp3.1/publish/win-x64/.", "./"]

ENV HOME ${SHOT_LICENSING_API_HOME}
ENV NAME shotlicapisvr

# Make ports available to the world outside this container for main web interface
EXPOSE ${HTTPS_PORT}

VOLUME C:/Shot.Licensing/Data

ENTRYPOINT ["dotnet", "Shot.Licensing.Api.exe"]