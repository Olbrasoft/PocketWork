# PocketWork

Webová aplikace pro správu objednávek a zákazníků - **Proof of Concept**

## ⚠️ Architektonické problémy (PoC)

Tento projekt slouží jako **proof of concept** a obsahuje několik **záměrných zjednodušení**, která **nejsou vhodná pro produkci**:

### 1. **Špatné pojmenování DbContext**

❌ **Problém:** DbContext by se měl jmenovat podle toho, že dědí z `DbContext`
- ✅ **Správně:** `PocketWorkDbContext` nebo `PwDbContext`
- ❌ **Špatně:** Obecné názvy jako `db`, `context`, `_context`

**Důvod:** Jasné pojmenování usnadňuje orientaci v kódu a reflektuje účel třídy.

### 2. **Entity v oddělené vrstvě od DbContext**

❌ **Problém:** Entity jsou v projektu `PocketWork.EntityFrameworkCore`, ale DbContext je v API projektu

**Proč je to špatná praxe:**
- Entity a DbContext spolu úzce souvisí a měly by být pohromadě
- Porušuje **Single Responsibility Principle** - datová vrstva by měla být samostatná
- Znesnadňuje testování a znovupoužití

✅ **Správně pro produkci:**
```
PocketWork.EntityFrameworkCore/
├── PocketWorkDbContext.cs      ← DbContext
├── Entities/                   ← Entity třídy
│   ├── User.cs
│   ├── Customer.cs
│   └── Order.cs
├── Configurations/             ← Fluent API konfigurace
└── Migrations/
```

### 3. **API nepředává data do MVC aplikace**

❌ **Problém:** V tomto PoC API slouží k předávání dat do MVC webu přes HTTP

**Proč je to špatná architektura:**
- **Zbytečná režie:** HTTP volání mezi vlastními službami na stejném serveru
- **Latence:** Každý request prochází síťovým stackem
- **Složitost:** Duplicitní error handling, serializace/deserializace

✅ **API by mělo sloužit pro:**
- **Tlusté klienty** (desktopové/mobilní aplikace)
- **Aplikace třetích stran** (integrace s externími systémy)
- **JavaScript frontends** (SPA - React, Vue, Angular)

✅ **Lepší architektura pro MVC + API:**

**Varianta A: Sdílená Business vrstva**
```
PocketWork.Business/          ← Služby, business logika
    ├── Services/
    └── Interfaces/

PocketWork.Mvc/               ← Web UI
    └── Uses Business.Services directly

PocketWork.Api/               ← REST API pro externí klienty
    └── Uses Business.Services directly

PocketWork.EntityFrameworkCore/ ← Datová vrstva
```

**Varianta B: MVC samostatně (žádné API)**
```
PocketWork.Mvc/
    ├── Controllers/          ← MVC controllery
    ├── Views/
    └── Uses DbContext directly (pro jednoduché CRUD)

PocketWork.EntityFrameworkCore/
```

**Varianta C: Pouze API + SPA frontend**
```
PocketWork.Api/               ← Backend API
PocketWork.Web/               ← React/Vue/Angular SPA
```

### 4. **Chybějící abstrakce**

❌ **V PoC chybí:**
- Repository Pattern (přímé použití DbContext)
- DTOs (API vrací přímo entity)
- Business vrstva (business logika v controllerech)
- Validace
- Error handling middleware

✅ **Pro produkci přidat:**
- Repository/Unit of Work pattern pro testovatelnost
- DTOs pro oddělení API kontraktu od databázových entit
- Business vrstvu pro komplexní logiku
- FluentValidation pro validaci
- Global exception handling

---

## 🎯 Účel tohoto PoC

Tento projekt demonstruje **základní strukturu .NET 10 aplikace** s:
- Entity Framework Core
- SQLite databází (pro snadný vývoj)
- MVC a API projekty
- Dodržením engineering-handbook standardů (adresářová struktura)

**Není určen k produkčnímu nasazení bez značných úprav!**

---

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQLite (součást .NET)

### Installation
```bash
git clone https://github.com/Olbrasoft/PocketWork.git
cd PocketWork
dotnet build
```

### Running the API
```bash
cd src/PocketWork.Api
dotnet run
```

API bude dostupné na: `https://localhost:5001` (nebo port uvedený v terminálu)

### Running the MVC Web
```bash
cd src/PocketWork.Mvc
dotnet run
```

Web bude dostupný na: `https://localhost:5002` (nebo port uvedený v terminálu)

### Running Tests
```bash
dotnet test
```

---

## Project Structure

```
PocketWork/
├── src/
│   ├── PocketWork.EntityFrameworkCore/   # Entities, DbContext, Configurations
│   ├── PocketWork.Api/                   # REST API
│   └── PocketWork.Mvc/                   # MVC Web Application
├── test/
│   ├── PocketWork.EntityFrameworkCore.Tests/
│   ├── PocketWork.Api.Tests/
│   └── PocketWork.Mvc.Tests/
├── .gitignore
├── LICENSE
├── README.md
└── PocketWork.sln
```

---

## License

MIT License - see LICENSE file for details.

---

## Documentation

Pro detailní architektonický návrh viz: `/home/jirka/Dokumenty/ProofOfConcepts/PocketWork.md`
