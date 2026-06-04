# Device Fleet Manager

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

🚧 W budowie. Plan rozwoju:

- [x] Etap 0 — repo + solucja
- [ ] Etap 1 — szkielet API + pierwszy endpoint (`GET /api/devices`)
- [ ] Etap 2 — baza danych (EF Core) + CRUD
- [ ] Etap 3 — architektura warstwowa + DTO/walidacja
- [ ] Etap 4 — security (JWT, role, rate limiting)
- [ ] Etap 5 — testy (xUnit)
- [ ] Etap 6 — Docker / Kubernetes / CI

## Uruchomienie (docelowo)

```bash
docker compose up        # API + baza danych
# Swagger: http://localhost:8080/swagger
```
