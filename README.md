# 📦 ProductApp

**ProductApp** to aplikacja webowa napisana w technologii **ASP.NET Core MVC**, która umożliwia:

- zarządzanie produktami (CRUD) w lokalnej bazie danych SQLite,
- wysyłanie i odbieranie danych JSON z zewnętrznego API przy pomocy `HttpClient`.

---

## ⚙️ Technologie

- **ASP.NET Core 8.0**
- **Entity Framework Core 9.0.5** 
- **SQLite (pakiet Microsoft.EntityFrameworkCore.Sqlite 9.0.5)**
- **MVC (Model-View-Controller)**
- **C# 12**
- **Razor Views**

---

## 🧩 Wymagania i cele

- Utworzenie bazy danych SQLite ze tabelą Product zawierającą pola: Id, Name, Price, Description.
- Implementacja modeli danych odpowiadających encjom Product oraz strukturze JSON (PostData).
- Stworzenie klasy ProductContext dziedziczącej po DbContext, do komunikacji z bazą.
- Wdrożenie pełnego zestawu operacji CRUD w kontrolerze ProductsController.
- Stworzenie kontrolera ApiDemoController, który:
  - wykonuje zapytanie GET do https://jsonplaceholder.typicode.com/posts/1,
  - wyświetla otrzymany JSON w widoku i w konsoli,
  - przyjmuje dane z formularza, serializuje je do JSON i wysyła POST do https://jsonplaceholder.typicode.com/posts,
  - wyświetla wynik POST w widoku i w konsoli.
- Obsługa błędów w każdej operacji na bazie i przy wywołaniach HTTP.

---

## 🗂 Struktura projektu

- `Controllers/` – klasa `ProductsController` obsługuje CRUD; klasa `ApiDemoController` obsługuje GET/POST do zewnętrznego API.
- `Data/` – `ProductContext` dziedziczy po `DbContext` i udostępnia `DbSet<Product>`.
- `Models/` – `Product` ma właściwości `Id`, `Name`, `Price`, `Description`; `PostData` ma struktury zgodne z JSON-em z API.
- `Views/` – widoki Razor rozdzielone na foldery `ApiDemo` i `Products`.
- Pliki konfiguracyjne (`appsettings.json`) zawierają connection string do pliku SQLite (`products.db`).

---

## 🧾 Modele danych

### Product

- `Id` (int) – klucz główny, autoinkrementowany.
- `Name` (string) – nazwa produktu.
- `Price` (decimal) – cena produktu.
- `Description` (string) – opis produktu.

### PostData

- `UserId` (int) – identyfikator użytkownika w zewnętrznym API.
- `Title` (string) – tytuł posta.
- `Body` (string) – treść posta.

---

## 💾 Kontekst bazy danych

### ProductContext – klasa dziedzicząca po DbContext

- `DbSet<Product> Products` – reprezentuje tabelę produktów w pliku SQLite.

---

## 🔄 Operacje CRUD dla produktów

W kontrolerze `ProductsController` znajdują się metody:

- `Index` – pobiera wszystkie produkty z bazy i wyświetla w widoku listy.
- `Details` – pobiera pojedynczy produkt po `Id` i przekazuje do widoku szczegółów.
- `Create (GET)` – zwraca widok formularza do utworzenia nowego produktu.
- `Create (POST)` – zapisuje nowy produkt do bazy.
- `Edit (GET)` – ładuje istniejący produkt po `Id` i przekazuje do widoku edycji.
- `Edit (POST)` – zapisuje zmienione dane do bazy.
- `Delete (GET)` – ładuje dane produktu do usunięcia i wyświetla stronę potwierdzenia.
- `DeleteConfirmed (POST)` – usuwa produkt po `Id` i przekierowuje na listę produktów.

### Widoki CRUD (Razor) w `Views/Products/`:

- `Index.cshtml` – tabela z listą produktów, linki do edycji, szczegółów, usunięcia.
- `Details.cshtml` – wyświetla wszystkie właściwości wybranego produktu.
- `Create.cshtml` i `Edit.cshtml` – formularze z polami `Name`, `Price`, `Description` i walidacją.
- `Delete.cshtml` – strona prosząca o potwierdzenie usunięcia.

---

## 🌐 Komunikacja z zewnętrznym API

W kontrolerze `ApiDemoController` wdrożono pobieranie i wysyłanie danych do API:

### 1. `GET /ApiDemo/GetPost`
- Wysyła zapytanie GET do `https://jsonplaceholder.typicode.com/posts/1`.
- Po otrzymaniu odpowiedzi (JSON) wypisuje ją w konsoli i przekazuje tekst do widoku `ApiResult`.

### 2. `GET /ApiDemo/CreatePost`
- Zwraca widok z formularzem, w którym użytkownik może wprowadzić `UserId`, `Title` i `Body`.

### 3. `POST /ApiDemo/CreatePost`
- Odbiera z formularza obiekt `PostData`.
- Serializuje go do JSON i wysyła do `https://jsonplaceholder.typicode.com/posts` metodą POST.
- Po otrzymaniu odpowiedzi JSON wypisuje wynik w konsoli i przekazuje tekst do widoku `ApiResult`.

### Widoki API Demo (`Views/ApiDemo/`):

- `Index.cshtml` – strona startowa z przyciskami GET i POST.
- `CreatePost.cshtml` – formularz do wypełnienia danych i wysyłki POST.
- `ApiResult.cshtml` – widok wyświetlający surowy JSON otrzymany w odpowiedzi (GET lub POST).
