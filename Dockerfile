# =========================================================================
# ETAP 1: BUILD — używamy pełnego SDK do skompilowania aplikacji
# =========================================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Najpierw kopiujemy same pliki .csproj i robimy restore.
# Dzięki temu Docker cache'uje pobrane pakiety NuGet i nie pobiera ich
# ponownie przy każdej zmianie kodu (przyspiesza kolejne buildy).
COPY DeviceFleet.Domain/*.csproj          DeviceFleet.Domain/
COPY DeviceFleet.Application/*.csproj     DeviceFleet.Application/
COPY DeviceFleet.Infrastructure/*.csproj  DeviceFleet.Infrastructure/
COPY DeviceFleet.Api/*.csproj             DeviceFleet.Api/
RUN dotnet restore DeviceFleet.Api/DeviceFleet.Api.csproj

# Teraz kopiujemy resztę kodu i publikujemy w konfiguracji Release.
COPY . .
RUN dotnet publish DeviceFleet.Api/DeviceFleet.Api.csproj -c Release -o /app --no-restore

# =========================================================================
# ETAP 2: RUNTIME — lekki obraz z samym runtime ASP.NET (bez SDK)
# =========================================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Kontener nasłuchuje na porcie 8080 (domyślny dla ASP.NET w kontenerze).
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Kopiujemy TYLKO gotowy wynik kompilacji z etapu build.
COPY --from=build /app .

ENTRYPOINT ["dotnet", "DeviceFleet.Api.dll"]
