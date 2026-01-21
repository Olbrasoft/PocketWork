# PocketWork

Vícevrstvá webová a desktopová aplikace pro správu objednávek a zákazníků.

## 📋 Přehled projektu

PocketWork je referenční implementace vícevrstvé .NET aplikace demonstrující moderní architektonické vzory a best practices pro vývoj enterprise aplikací.

### Hlavní charakteristiky

- **Vícevrstvá architektura** s jasně oddělenými odpovědnostmi
- **Repository Pattern** s DTOs pro abstrakci datové vrstvy
- **Cross-platform** - Web (MVC), API a Desktop (Avalonia UI)
- **Entity Framework Core** s SQLite pro snadný vývoj
- **Clean Architecture** principy
- **Testovatelnost** - každý projekt má odpovídající test projekt

---

## 🏗️ Architektura

### Vrstvený model

```
                    ┌─────────────────────────┐
                    │   PocketWork.Desktop    │
                    │   (Avalonia UI)         │
                    │   Win/Linux/Mac         │
                    └───────────┬─────────────┘
                                │
                                │ HTTP/REST (externí klient)
                                ▼
┌───────────────────────────────────────────────────────────────────────────┐
│                              SERVER                                        │
├───────────────────────────────┬───────────────────────────────────────────┤
│                               │                                           │
│   ┌───────────────────┐       │       ┌───────────────────┐               │
│   │  PocketWork.Mvc   │       │       │  PocketWork.Api   │               │
│   │  (Web UI)         │       │       │  (REST API)       │               │
│   │  Server-side HTML │       │       │  JSON endpointy   │               │
│   └─────────┬─────────┘       │       └─────────┬─────────┘               │
│             │                 │                 │                         │
│             │ ProjectReference│  ProjectReference                         │
│             │                 │                 │                         │
│             └────────┬────────┴────────┬────────┘                         │
│                      │                 │                                  │
│                      ▼                 ▼                                  │
│          ┌──────────────────────────────────────────────┐                 │
│          │         PocketWork.Repositories              │                 │
│          │         (Repository Pattern + DTOs)          │                 │
│          │                                              │                 │
│          │  • IOrderRepository, ICustomerRepository     │                 │
│          │  • CreateOrderDto, OrderResponseDto          │                 │
│          │  • Mapování Entity ↔ DTO                     │                 │
│          └──────────────────┬───────────────────────────┘                 │
│                             │                                             │
│                             │ ProjectReference                            │
│                             ▼                                             │
│          ┌──────────────────────────────────────────────┐                 │
│          │      PocketWork.EntityFrameworkCore          │                 │
│          │      (Datová vrstva)                         │                 │
│          │                                              │                 │
│          │  • Entity: User, Customer, Order, ServiceType│                 │
│          │  • PocketWorkDbContext                       │                 │
│          │  • Fluent API Configurations                 │                 │
│          └──────────────────┬───────────────────────────┘                 │
│                             │                                             │
│                             │ SQL                                         │
│                             ▼                                             │
│                   ┌─────────────────┐                                     │
│                   │     SQLite      │                                     │
│                   │    Database     │                                     │
│                   └─────────────────┘                                     │
└───────────────────────────────────────────────────────────────────────────┘
```

**Klíčové body:**
- **Desktop** je **mimo server** - komunikuje s API přes HTTP/REST
- **MVC** a **API** běží **na serveru** a mají přímou `ProjectReference` na Repositories
- **MVC** a **API** jsou na sobě **nezávislé** (žádná reference mezi nimi)
- **Repositories** má `ProjectReference` na **EntityFrameworkCore**

---

## 📁 Struktura projektu

```
PocketWork/
├── src/                                          # Zdrojové kódy
│   ├── PocketWork.EntityFrameworkCore/           # Datová vrstva
│   │   ├── Entities/                             # Doménové entity
│   │   │   ├── User.cs
│   │   │   ├── Customer.cs
│   │   │   ├── ServiceType.cs
│   │   │   └── Order.cs
│   │   ├── Enums/                                # Enumy (JobType, OrderType)
│   │   ├── PocketWorkDbContext.cs                # DbContext
│   │   ├── Configurations/                       # Fluent API konfigurace
│   │   └── Migrations/                           # EF Core migrace
│   │
│   ├── PocketWork.Repositories/                  # Repository vrstva
│   │   ├── DTOs/                                 # Data Transfer Objects
│   │   │   ├── Orders/
│   │   │   │   ├── CreateOrderDto.cs
│   │   │   │   ├── UpdateOrderDto.cs
│   │   │   │   └── OrderResponseDto.cs
│   │   │   ├── Customers/
│   │   │   └── Users/
│   │   ├── Repositories/                         # Repository implementace
│   │   │   ├── OrderRepository.cs
│   │   │   ├── CustomerRepository.cs
│   │   │   ├── UserRepository.cs
│   │   │   └── ServiceTypeRepository.cs
│   │   └── Interfaces/                           # Repository rozhraní
│   │       ├── IRepository.cs
│   │       ├── IOrderRepository.cs
│   │       ├── ICustomerRepository.cs
│   │       └── IUserRepository.cs
│   │
│   ├── PocketWork.Api/                           # REST API
│   │   ├── Controllers/                          # API controllery
│   │   │   ├── OrdersController.cs
│   │   │   ├── CustomersController.cs
│   │   │   └── UsersController.cs
│   │   ├── Middleware/                           # Custom middleware
│   │   └── Program.cs
│   │
│   ├── PocketWork.Mvc/                           # MVC webová aplikace
│   │   ├── Controllers/                          # MVC controllery
│   │   ├── Views/                                # Razor views
│   │   ├── Services/                             # API client (pro PoC)
│   │   ├── wwwroot/                              # Statické soubory
│   │   └── Program.cs
│   │
│   └── PocketWork.Desktop/                       # Desktop aplikace
│       ├── Views/                                # Avalonia XAML views
│       ├── ViewModels/                           # MVVM ViewModels
│       ├── Services/                             # API client
│       │   └── ApiClient.cs
│       └── Program.cs
│
├── test/                                         # Unit testy
│   ├── PocketWork.EntityFrameworkCore.Tests/
│   ├── PocketWork.Repositories.Tests/
│   ├── PocketWork.Api.Tests/
│   ├── PocketWork.Mvc.Tests/
│   └── PocketWork.Desktop.Tests/
│
├── .github/workflows/                            # CI/CD
├── deploy/                                       # Deployment skripty
├── .gitignore
├── LICENSE
├── README.md
├── AGENTS.md
└── PocketWork.sln
```

---

## 🧩 Komponenty - Podrobný popis

### 1. PocketWork.EntityFrameworkCore (Datová vrstva)

**Účel:** Toto je nejnižší vrstva aplikace, která se stará o veškerou komunikaci s databází.

**Co dělá:**
- **Mapování na entity** - Definuje doménové entity (`User`, `Customer`, `Order`, `ServiceType`), které odpovídají tabulkám v databázi. Každá entita je C# třída s vlastnostmi, které se mapují na sloupce databázové tabulky.
- **DbContext** - Třída `PocketWorkDbContext` je "brána" do databáze. Obsahuje `DbSet<T>` kolekce pro každou entitu a řídí připojení k databázi, sledování změn a ukládání dat.
- **Fluent API konfigurace** - Ve složce `Configurations/` jsou třídy, které přesně definují, jak se entity mapují na databázi (názvy sloupců, datové typy, relace mezi tabulkami, indexy, omezení).
- **Migrace** - EF Core sleduje změny v entitách a generuje SQL příkazy pro aktualizaci schématu databáze.

**Proč existuje samostatně:**
Oddělení datové vrstvy umožňuje:
- Změnit databázi (SQLite → PostgreSQL → SQL Server) bez změn v ostatních vrstvách
- Testovat vyšší vrstvy s mock databází
- Verzovat schéma databáze pomocí migrací

```
Entity → DbContext → SQL dotazy → SQLite/PostgreSQL/SQL Server
```

---

### 2. PocketWork.Repositories (Repository vrstva / Vrstva repozitářů)

**Účel:** Tato vrstva je **klíčová abstrakce**, která odděluje zbytek aplikace od přímého přístupu k databázi.

**Problém, který řeší:**
Bez této vrstvy by controllery (MVC, API) měly přímý přístup k `DbContext`. To je problematické:
- Controller by mohl vykonat libovolný SQL dotaz
- Databázové entity by "prosakovaly" do API odpovědí
- Změna v databázi by vyžadovala změny v controllerech

**Jak to funguje:**

1. **Repository třídy** (`OrderRepository`, `CustomerRepository`, ...) mají přístup k `DbContext`, ale ven vystavují pouze definované metody:
   ```csharp
   public interface IOrderRepository
   {
       Task<IEnumerable<OrderResponseDto>> GetAllAsync();
       Task<OrderResponseDto?> GetByIdAsync(int id);
       Task<OrderResponseDto> CreateAsync(CreateOrderDto dto);
       Task DeleteAsync(int id);
   }
   ```

2. **DTOs (Data Transfer Objects)** - Do vyšších vrstev se **nedostávají entity**, ale pouze DTOs:
   - `CreateOrderDto` - data pro vytvoření objednávky (vstup)
   - `UpdateOrderDto` - data pro aktualizaci (vstup)
   - `OrderResponseDto` - data vrácená zpět (výstup)

**Proč DTOs místo entit:**
- **Bezpečnost** - Můžete kontrolovat, která data se posílají ven
- **Flexibilita** - DTO může kombinovat data z více entit (např. `CustomerName` v `OrderResponseDto`)
- **Stabilita API** - Změna entity neovlivní API kontrakt
- **Výkon** - DTO obsahuje jen potřebná data, ne celou entitu s navigačními vlastnostmi

```
Controller → Repository.GetAllAsync() → [OrderResponseDto, ...]
                    ↓
            DbContext.Orders → mapování → DTO
```

---

### 3. PocketWork.Api (REST API vrstva)

**Účel:** Poskytuje HTTP endpointy pro aplikace, které **běží mimo server** - tedy nemají přímý přístup k databázi ani k repository vrstvě.

**Kdy se používá:**
- **Desktopové aplikace** - běží na počítači uživatele
- **Mobilní aplikace** - běží na telefonu/tabletu
- **Single Page Applications (SPA)** - React, Vue.js, Angular aplikace
- **Integrace třetích stran** - jiné systémy, které potřebují přístup k datům
- **Mikroslužby** - komunikace mezi jednotlivými službami

**Co poskytuje:**
- RESTful endpointy: `GET /api/orders`, `POST /api/customers`, atd.
- JSON formát pro přenos dat
- Swagger/OpenAPI dokumentace na `/swagger`
- HTTP status kódy pro signalizaci výsledku (200 OK, 404 Not Found, 400 Bad Request)

**Architektura:**
```
Desktop App ──HTTP──→ API Controller ──→ Repository ──→ DbContext ──→ DB
Mobile App  ──HTTP──↗
3rd Party   ──HTTP──↗
```

**Důležité:** API vrstva pracuje pouze s DTOs z Repository vrstvy. Nikdy nevrací přímo databázové entity.

---

### 4. PocketWork.Mvc (Webová prezentační vrstva)

**Účel:** Server-side webová aplikace, která generuje HTML stránky na serveru a posílá je do prohlížeče.

**Co je MVC:**
- **Model** - data (v našem případě DTOs z Repository)
- **View** - Razor šablony (.cshtml), které generují HTML
- **Controller** - řídí tok dat mezi Model a View

**Jak to funguje:**
1. Uživatel zadá URL v prohlížeči
2. Server zpracuje požadavek v Controlleru
3. Controller načte data z Repository
4. Data se předají do View (Razor šablony)
5. View vygeneruje HTML
6. HTML se pošle do prohlížeče

**Alternativy k MVC:**
MVC je jen **jedna z možností** webové prezentace. Microsoft nabízí další:

| Technologie | Popis | Použití |
|------------|-------|---------|
| **MVC** | Model-View-Controller | Komplexní webové aplikace |
| **Razor Pages** | Stránkově orientovaný model | Jednodušší weby, formuláře |
| **Blazor Server** | C# komponenty na serveru | Interaktivní aplikace bez JS |
| **Blazor WebAssembly** | C# běžící v prohlížeči | SPA bez JS |
| **Minimal APIs** | Lehké HTTP handlery | Mikroslužby, jednoduchá API |

**MVC vs Razor Pages:**
- MVC má oddělené Controllers a Views - vhodné pro větší aplikace
- Razor Pages kombinují logiku a view do jednoho souboru - jednodušší pro CRUD operace

**V této aplikaci:** MVC přistupuje **přímo k Repository vrstvě** (ne přes API), protože běží na stejném serveru jako databáze.

---

### 5. PocketWork.Desktop (Desktopová aplikace)

**Účel:** Cross-platform desktopová aplikace, která běží na počítači uživatele (Windows, Linux, macOS).

**Technologie:**
- **Avalonia UI** - moderní cross-platform UI framework pro .NET (obdoba WPF pro všechny platformy)
- **MVVM pattern** - Model-View-ViewModel architektura
- **CommunityToolkit.Mvvm** - knihovna pro zjednodušení MVVM

**Jak komunikuje s daty:**
Desktop aplikace **nemá přímý přístup k databázi** (ta běží na serveru). Místo toho:
1. Posílá HTTP požadavky na API (`http://localhost:5050/api/...`)
2. Přijímá JSON data
3. Deserializuje je do lokálních modelů
4. Zobrazuje v UI

```
Desktop App ←──JSON──→ REST API ←──→ Repository ←──→ Database
   (UI)                (Server)        (Server)       (Server)
```

**Struktura:**
- `Views/` - XAML soubory definující UI (tlačítka, tabulky, formuláře)
- `ViewModels/` - C# třídy s logikou a daty pro UI
- `Models/` - lokální modely pro API odpovědi
- `Services/ApiClient.cs` - HTTP klient pro komunikaci s API

**MVVM výhody:**
- Čistě oddělené UI od logiky
- Snadné unit testování (ViewModels lze testovat bez UI)
- Data binding - automatická synchronizace mezi UI a daty

---

## 🚀 Začínáme

### Požadavky
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQLite (zahrnuté v .NET)
- (Volitelně) Git

### Instalace

```bash
# Klonování repozitáře
git clone https://github.com/Olbrasoft/PocketWork.git
cd PocketWork

# Build celého řešení
dotnet build

# Spuštění testů
dotnet test
```

### Vytvoření databáze

```bash
cd src/PocketWork.Api
dotnet ef migrations add InitialCreate --project ../PocketWork.EntityFrameworkCore
dotnet ef database update
```

### Spuštění aplikací

#### REST API
```bash
cd src/PocketWork.Api
dotnet run
```
API: `https://localhost:5001` (Swagger: `/swagger`)

#### MVC Web
```bash
cd src/PocketWork.Mvc
dotnet run
```
Web: `https://localhost:5002`

#### Desktop aplikace
```bash
cd src/PocketWork.Desktop
dotnet run
```

---

## 🧪 Testování

Projekt obsahuje komplexní unit testy pro všechny vrstvy:

```bash
# Spuštění všech testů
dotnet test

# Testy pro konkrétní projekt
dotnet test test/PocketWork.Repositories.Tests

# S pokrytím kódu
dotnet test /p:CollectCoverage=true
```

**Test framework:** xUnit + Moq + Microsoft.EntityFrameworkCore.InMemory

---

## 🛠️ Technologie

| Vrstva | Technologie |
|--------|-------------|
| **Framework** | .NET 10 |
| **ORM** | Entity Framework Core 10 |
| **Database** | SQLite (dev), PostgreSQL/SQL Server (prod) |
| **Web** | ASP.NET Core MVC |
| **API** | ASP.NET Core Web API |
| **Desktop** | Avalonia UI 11.2 |
| **Testing** | xUnit, Moq |
| **Architecture** | Repository Pattern, DTO Pattern, MVC (Web), MVVM (Desktop) |

---

## 📚 Architektonické vzory

### Repository Pattern
Odděluje business logiku od datového přístupu pomocí abstrakce:
```csharp
public interface IOrderRepository : IRepository<Order, CreateOrderDto, UpdateOrderDto, OrderResponseDto>
{
    Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerIdAsync(int customerId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime from, DateTime to);
}
```

### DTO Pattern
Data Transfer Objects izolují API kontrakt od databázového modelu:
```csharp
public record OrderResponseDto
{
    public int Id { get; init; }
    public string? CustomerName { get; init; }  // Sestaveno z entity
    public OrderType OrderType { get; init; }
    // ...
}
```

### MVVM (Desktop)
Model-View-ViewModel pro Avalonia desktop aplikaci:
```csharp
public class OrdersViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    public ObservableCollection<Order> Orders { get; set; }
    public ICommand LoadOrdersCommand { get; }
}
```

---

## 🔧 Konfigurace

### Connection String (SQLite - Development)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=pocketwork.db"
  }
}
```

### Přepnutí na PostgreSQL (Production)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=pocketwork;Username=user;Password=pass"
  }
}
```

Změnit v `.csproj`:
```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.*" />
```

---

## 📖 Dokumentace

- **Engineering Handbook:** [GitHub Engineering Handbook](https://github.com/Olbrasoft/engineering-handbook)
- **AGENTS.md:** Instrukce pro AI agenty
- **Proof of Concept:** `/home/jirka/Dokumenty/ProofOfConcepts/PocketWork.md`

## 🎬 Video

Záznam z vývoje projektu pomocí AI (Claude Code):

[![PocketWork Development](https://img.youtube.com/vi/GruRbL-06cA/0.jpg)](https://www.youtube.com/watch?v=GruRbL-06cA)

---

## 🤝 Příspěvky

Projekt slouží jako referenční implementace a demo. Pro příspěvky:
1. Fork repozitáře
2. Vytvoř feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit změny (`git commit -m 'Add: Amazing Feature'`)
4. Push do branch (`git push origin feature/AmazingFeature`)
5. Otevři Pull Request

---

## 📄 Licence

MIT License - viz [LICENSE](LICENSE) soubor.

---

## 👤 Autor

**Olbrasoft**
- GitHub: [@Olbrasoft](https://github.com/Olbrasoft)

---

## 🙏 Poděkování

- [Avalonia UI](https://avaloniaui.net/) - Cross-platform UI framework
- [Entity Framework Core](https://docs.microsoft.com/ef/core/) - ORM
- Microsoft .NET Team
