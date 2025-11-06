FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY . /src

RUN dotnet restore "./karg.API/karg.API.csproj"

WORKDIR "/src/karg.API"

RUN dotnet publish "./karg.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

WORKDIR /app
COPY --from=build /app/publish /app
EXPOSE 8080

ENTRYPOINT ["dotnet", "karg.API.dll"]
