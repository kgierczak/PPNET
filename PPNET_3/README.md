# PPNET_3 - Analiza Wydajności Mnożenia Macierzy

Projekt badawczy mający na celu **porównanie wydajności wykonywania zadań obliczeniowych na jednym lub wielu wątkach**.

## Cel Projektu

Analiza różnic w wydajności między trzema podejściami do mnożenia macierzy:
- **Sequential** - mnożenie sekwencyjne (1 wątek)
- **Parallel.For** - wielowątkowość zarządzana przez .NET (Parallel.For)
- **Thread** - ręczne zarządzanie wątkami

## Struktura

`
PPNET_3/
├── PPNET_3/                    # Główna aplikacja MAUI
└── PPNET_3/MatrixMultiplication/   # Projekt benchmark
    ├── Matrix.cs              # Implementacja operacji macierzowych
    ├── Benchmark.cs           # Pomiary wydajności i uruchamianie testów
    └── Program.cs             # Punkt wejścia
`

## Funkcjonalności

- **Benchmark wielowątkowy** - testy dla różnych rozmiarów macierzy (100×100, 200×200, 500×500)
- **Pomiar wydajności** - średni czas wykonania i przyspieszenie (speedup)
- **Weryfikacja poprawności** - sprawdzenie zgodności wyników między metodami
- **Konfigurowalne parametry** - liczba wątków (1, 2, 4, 8, 14)

## Uruchomienie

`bash
cd PPNET_3/PPNET_3/MatrixMultiplication
dotnet run
`

## Wyniki

Program generuje raporty zawierające:
- Średni czas wykonania dla każdej metody
- Współczynnik przyspieszenia (speedup) względem metody sekwencyjnej
- Weryfikację poprawności obliczeń
