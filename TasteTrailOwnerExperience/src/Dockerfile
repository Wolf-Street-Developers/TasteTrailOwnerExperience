# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ./TasteTrailOwnerExperience.Api/*.csproj ./TasteTrailOwnerExperience.Api/
COPY ./TasteTrailOwnerExperience.Infrastructure/*.csproj ./TasteTrailOwnerExperience.Infrastructure/
COPY ./TasteTrailOwnerExperience.Core/*.csproj ./TasteTrailOwnerExperience.Core/

COPY . .

RUN dotnet publish TasteTrailOwnerExperience.Api/*.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TasteTrailOwnerExperience.Api.dll"]
