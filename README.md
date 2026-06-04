# Device Fleet Manager

[![CI](https://github.com/Cinkut/DeviceFleetManager/actions/workflows/ci.yml/badge.svg)](https://github.com/Cinkut/DeviceFleetManager/actions/workflows/ci.yml)

Cloud‑owy serwis do provisioningu i zarządzania flotą urządzeń (rejestracja urządzeń,
grupowanie we floty, wysyłanie komend konfiguracyjnych i aktualizacji).

Projekt portfolio demonstrujący nowoczesny stack .NET: ASP.NET Core Web API,
EF Core + relacyjna baza danych, architektura warstwowa, autoryzacja JWT,
testy oraz konteneryzacja.

## Stack technologiczny

- **.NET 10** / C#
- **ASP.NET Core Web API** (REST + OpenAPI/Swagger)
- **Entity Framework Core** + relacyjna baza danych
- **JWT** (autoryzacja, role) — adresowanie ryzyk OWASP Top 10
- **xUnit** (testy jednostkowe i integracyjne)
- **Docker** / docker-compose (+ opcjonalnie Kubernetes)
- **GitHub Actions** (CI: build + testy + obraz Docker)

## Architektura

```
DeviceFleet.Domain          # encje i logika domenowa
DeviceFleet.Application      # serwisy / przypadki użycia (DI)
DeviceFleet.Infrastructure   # EF Core, repozytoria, dostęp do danych
DeviceFleet.Api              # kontrolery REST, konfiguracja, auth
DeviceFleet.Tests            # testy jednostkowe i integracyjne
```

## Status

✅ **Ukończony** — wszystkie zaplanowane funkcjonalności zrealizowane.

- [x] Web API + endpointy REST (`/api/devices`, `/api/auth`)
- [x] Baza danych (EF Core + PostgreSQL) + pełny CRUD + migracje
- [x] Architektura warstwowa (Domain / Application / Infrastructure / Api) + DTO i walidacja
- [x] Security — JWT, role (Admin / Operator), rate limiting, OWASP Top 10
- [x] Testy jednostkowe (xUnit + Moq)
- [x] Konteneryzacja (Docker multi-stage + docker-compose) z automatycznymi migracjami
- [x] CI — GitHub Actions (build + testy + budowa obrazu Docker)

## Bezpieczeństwo (OWASP Top 10)

Projekt świadomie adresuje wybrane ryzyka z OWASP Top 10:

| Ryzyko | Zastosowane zabezpieczenie |
|---|---|
| A01 Broken Access Control | Autoryzacja oparta o role — odczyt dla zalogowanych, zapis tylko dla `Admin` (`[Authorize(Roles="Admin")]`) |
| A02 Cryptographic Failures | Tokeny JWT podpisywane HMAC-SHA256; klucz poza kodem (konfiguracja / sekrety) |
| A03 Injection | EF Core (parametryzowane zapytania) + walidacja wejścia (DataAnnotations) |
| A05 Security Misconfiguration | Walidacja issuer/audience/lifetime tokenu, wymuszanie HTTPS |
| A07 Identification & Auth Failures | Centralne logowanie z wydawaniem tokenów, ograniczony czas życia tokenu |
| Brute-force / DoS | Rate limiting (fixed window, 100 żądań / min) |

> Uwaga: użytkownicy logowania są demonstracyjni (hardcoded). W produkcji
> trafiliby do bazy z **zahaszowanymi** hasłami (np. ASP.NET Identity / bcrypt).

## Uruchomienie

### Opcja A — wszystko w Dockerze (zalecane)

```bash
docker compose up -d --build         # buduje API + startuje API i bazę
# API + Swagger: http://localhost:8080/swagger
```
Migracje bazy danych wykonują się automatycznie przy starcie API.

### Opcja B — API lokalnie, baza w Dockerze

```bash
docker compose up -d db              # tylko PostgreSQL (port 5433)
cd DeviceFleet.Api
dotnet run                           # Swagger: http://localhost:<port>/swagger
```

### Logowanie (demo)

```
POST /api/auth/login
{ "username": "admin",    "password": "admin123" }      # rola Admin (pełny dostęp)
{ "username": "operator", "password": "operator123" }   # rola Operator (tylko odczyt)
```

W Swaggerze użyj przycisku **Authorize** i wklej otrzymany token.

## Testy

```bash
dotnet test
```
