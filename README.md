# FlightManagementAPI

Prosty interfejs API do obsługi lotów.

Aby aplikacja działała należy utworzyć bazę danych.
Instrukcja tworzenia bazy w visual studio -> Menu Bar -> Tools -> NuGet Package Manager -> Package Manager Console -> Pojawi się konsola w której trzeba wybrać FlightManagementData jako Default project oraz wpisać komendę update-database -> powinna utworzyć się baza danych

Testy jednostkowe dla kontrolerów i serwisów znajdują się w FlightManagementAPI.Tests, zostały napisane za pomocą xunit.

Aplikacja została stworzona w ASP.NET Core Web API, wersja .NET 8.0.
Dodatkowo użyto:
- Entity Framework
- Identity
- AutoFixture
- Moq
