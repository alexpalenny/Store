FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY SeaStore/SeaStore.csproj SeaStore/
RUN dotnet restore SeaStore/SeaStore.csproj
COPY . .
WORKDIR /src/SeaStore
RUN dotnet build SeaStore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SeaStore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SeaStore.dll"]
