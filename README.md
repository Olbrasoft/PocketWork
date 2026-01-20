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
┌─────────────────────────────────────────────────────┐
│                  PocketWork.Desktop                 │
│              (Avalonia UI - Linux/Win/Mac)          │
└────────────────────┬────────────────────────────────┘
                     │ HTTP/REST
                     ▼
┌─────────────────────────────────────────────────────┐
│              PocketWork.Api (REST API)              │
└────────────────────┬────────────────────────────────┘
                     │
        ┌────────────┴────────────┐
        │                         │
        ▼                         ▼
┌──────────────┐          ┌──────────────┐
│ PocketWork.  │          │ PocketWork.  │
│     Mvc      │          │     Api      │
│ (Web UI)     │          │ (REST API)   │
└──────┬───────┘          └──────┬───────┘
       │                         │
       └────────────┬────────────┘
                    │ Repository calls
                    ▼
        ┌─────────────────────────┐
        │  PocketWork.Repositories │
        │  (DTOs + Repositories)   │
        └────────────┬─────────────┘
                     │ DbContext access
                     ▼
        ┌─────────────────────────┐
        │ PocketWork.EntityFramework│
        │         Core              │
        │ (Entities, DbContext)     │
        └────────────┬─────────────┘
                     │ SQL queries
                     ▼
              ┌─────────────┐
              │   SQLite    │
              │  Database   │
              └─────────────┘
```

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

## 🧩 Komponenty

### 1. PocketWork.EntityFrameworkCore
**Datová vrstva** obsahující:
- **Entities:** Doménové entity (User, Customer, Order, ServiceType)
- **DbContext:** `PocketWorkDbContext` s Fluent API konfigurací
- **Migrations:** EF Core databázové migrace
- **Enums:** `JobType`, `OrderType`

### 2. PocketWork.Repositories
**Repository vrstva** poskytující:
- **DTOs:** Data Transfer Objects oddělující API od databázových entit
- **Repository Pattern:** Abstrakce nad databázovým přístupem
- **Interfaces:** `IRepository<T>`, `IOrderRepository`, atd.
- **Mapping:** Mapování mezi entitami a DTOs

### 3. PocketWork.Api
**REST API** pro externí klienty:
- RESTful endpointy pro CRUD operace
- Swagger/OpenAPI dokumentace
- Používá Repository vrstvu
- Navrženo pro Desktop a mobilní aplikace

### 4. PocketWork.Mvc
**MVC webová aplikace:**
- Server-side rendering s Razor views
- Bootstrap 5 UI
- Používá Repository vrstvu
- Responsive design

### 5. PocketWork.Desktop
**Cross-platform desktopová aplikace:**
- **Avalonia UI** framework (Linux, Windows, macOS)
- **MVVM** architektura
- Komunikuje s API přes HTTP
- Moderní Fluent Design

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
| **Architecture** | Repository Pattern, DTO Pattern, MVVM |

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
