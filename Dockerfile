# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Копируем только файлы проекта для восстановления зависимостей
COPY ./TasteTrailOwnerExperience/src/TasteTrailOwnerExperience.Api/*.csproj ./TasteTrailOwnerExperience.Api/
COPY ./TasteTrailOwnerExperience/src/TasteTrailOwnerExperience.Infrastructure/*.csproj ./TasteTrailOwnerExperience.Infrastructure/
COPY ./TasteTrailOwnerExperience/src/TasteTrailOwnerExperience.Core/*.csproj ./TasteTrailOwnerExperience.Core/

# Восстанавливаем зависимости
RUN dotnet restore ./TasteTrailOwnerExperience.Api/TasteTrailOwnerExperience.Api.csproj

# Копируем оставшиеся файлы исходного кода
COPY . .

# Публикуем проект
RUN dotnet publish ./TasteTrailOwnerExperience/src/TasteTrailOwnerExperience.Api/TasteTrailOwnerExperience.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Копируем только опубликованные файлы
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TasteTrailOwnerExperience.Api.dll"]
