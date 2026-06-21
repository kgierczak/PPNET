## Cel Projektu

Projekt demonstracyjny umożliwiający:
- Wyświetlanie prognozy pogody w formie tabeli
- Filtrowanie danych po temperaturze
- Wyszukiwanie po opisie warunków pogodowych
- Interaktywne komponenty Blazor z renderowaniem po stronie serwera

## Struktura Projektu

```
PPNET_4/
└── BlazorWeatherApp/
    ├── Components/
    │   ├── Pages/
    │   │   ├── Home.razor          # Strona główna
    │   │   ├── Weather.razor       # Komponent pogody (główny)
    │   │   ├── Counter.razor       # Licznik (demo)
    │   │   └── Error.razor         # Strona błędu
    │   └── App.razor               # Komponent główny aplikacji
    ├── wwwroot/
    │   ├── app.css                 # Style aplikacji
    │   └── lib/bootstrap/          # Biblioteka Bootstrap
    └── BlazorWeatherApp.csproj     # Definicja projektu
```

## Komponenty Główne

### Weather Component
- Wyświetlanie listy prognoz pogody
- Filtrowanie dni ciepłych (>15°C)
- Filtrowanie dni zimnych (<15°C)
- Wyszukiwanie po podsumowaniu warunków
- Przycisk Restore przywracający oryginalną listę

## Funkcjonalności

- ✅ Wyświetlanie prognozy pogody
- ✅ Filtrowanie temperaturowe
- ✅ Wyszukiwanie tekstowe
- ✅ Responsywny interfejs (Bootstrap 5)
- ✅ Renderowanie interaktywne po stronie serwera

## Uruchomienie

```bash
cd PPNET_4/BlazorWeatherApp
dotnet run
```

Aplikacja będzie dostępna pod adresem: `https://localhost:7258`
