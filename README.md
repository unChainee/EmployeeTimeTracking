# Employee Time Tracking API

## Wymagania

Przed uruchomieniem aplikacji upewnij się, że masz zainstalowane:
- .NET 8+
- Docker + Docker Compose
- PostgreSQL

## Konfiguracja Bazy Danych

### Opcja 1: Użycie Dockera
Jeśli chcesz uruchomić PostgreSQL w kontenerze Dockera, użyj `docker-compose.yml`. W katalogu projektu uruchom:

```sh
docker-compose up -d
```

Ten plik stworzy kontener PostgreSQL z domyślnymi ustawieniami:
- **Host**: `localhost`
- **Port**: `5432`
- **Użytkownik**: `postgres`
- **Hasło**: `123456`
- **Baza danych**: `EmployeeDB`

### Opcja 2: Ręczna konfiguracja PostgreSQL
Jeśli masz własną instancję PostgreSQL, utwórz bazę danych i zmień parametry połączenia w `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=employeetime;Username=postgres;Password=password"
}
```

## Uruchomienie Aplikacji

### 1. Przygotowanie środowiska
Po sklonowaniu repozytorium przejdź do katalogu projektu i zainstaluj zależności:

```sh
dotnet restore
```

### 2. Uruchomienie aplikacji
Aby uruchomić aplikację, użyj komendy:

```sh
dotnet run
```

Aplikacja powinna być dostępna pod adresem:

```
https://localhost:7122/swagger
```

## Dokumentacja API (Swagger)

Aby przetestować API, przejdź do [Swagger UI](http://localhost:7122/swagger) i użyj dostępnych endpointów.

## Autoryzacja

Aplikacja wykorzystuje **Basic Authentication**. W Swaggerze:
1. Kliknij **Authorize**.
2. Wpisz dane logowania:
   - **Username**: `admin`
   - **Password**: `password`
3. Zatwierdź i przetestuj zabezpieczone endpointy.

## Testy

### Uruchomienie testów jednostkowych i integracyjnych

Aby uruchomić testy, użyj komendy:

```sh
dotnet test
```

