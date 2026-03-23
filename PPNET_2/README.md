# Główne funkcjonalności

-> Wyszukiwanie posiłków: Integracja z zewnętrznym API do wyszukiwania dań. Wyszukane potrawy są automatycznie zapisywane w lokalnej bazie danych.
-> Baza lokalna (Offline): Zapisywanie posiłków i ich kategorii lokalnie.
-> Dodawanie ręczne: Możliwość ręcznego zdefiniowania własnego posiłku oraz przypisania mu nowej kategorii.
-> Zarządzanie i usuwanie: Łatwe usuwanie niechcianych posiłków z zapisanej listy.
-> Sortowanie: Możliwość alfabetycznego sortowania posiłków za pomocą przycisku.

# Struktura projektu

-> `Models/` - Modele danych używane przez Entity Framework (np. `Meal.cs`, `Category.cs`).
-> `ViewModels/` - Klasy zawierające logikę biznesową dla widoków. `MainViewModel` zarządza kolekcją potraw i obsługuje interakcje użytkownika.
-> `Data/` - Warstwa dostępu do danych, w tym główny kontekst EF Core (`AppDbContext.cs`).
-> `Services/` - Usługi pomocnicze takie jak `MealService` do integracji z zewnętrznym Web API dań.
-> `MainPage.xaml` - Główny interfejs graficzny aplikacji.
