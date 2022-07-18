#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app

ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  

COPY ["AzureBlobStorage.Infrastructure/", "AzureBlobStorage.Infrastructure/"]
COPY ["AzureBlobStoragePOC.API/", "AzureBlobStoragePOC.API/"]

WORKDIR /app/AzureBlobStoragePOC.API

RUN dotnet restore AzureBlobStoragePOC.API.csproj

RUN dotnet publish AzureBlobStoragePOC.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT dotnet AzureBlobStoragePOC.API.dll