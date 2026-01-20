# AGENTS.md

Instructions for AI agents working with PocketWork repository.

## Project Overview

**PocketWork** is a Proof of Concept web application for managing orders and customers.

**⚠️ Important:** This is a **simplified PoC** with intentional architectural shortcuts. Not production-ready!

### Architecture

```
PocketWork/
├── src/
│   ├── PocketWork.EntityFrameworkCore/  # Data layer (Entities, DbContext, EF Core)
│   ├── PocketWork.Api/                  # REST API
│   └── PocketWork.Mvc/                  # MVC Web Application
└── test/                                # Test projects (xUnit + Moq)
```

### Technology Stack

- **.NET 10**
- **Entity Framework Core 10**
- **SQLite** (development database)
- **ASP.NET Core MVC**
- **ASP.NET Core Web API**
- **xUnit + Moq** (testing)

---

## Build Commands

### Build Solution
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Run API
```bash
cd src/PocketWork.Api
dotnet run
```

### Run MVC Web
```bash
cd src/PocketWork.Mvc
dotnet run
```

### Create Migration
```bash
cd src/PocketWork.Api
dotnet ef migrations add MigrationName --project ../PocketWork.EntityFrameworkCore
```

### Update Database
```bash
cd src/PocketWork.Api
dotnet ef database update
```

---

## Code Style Guidelines

### .NET Standards (Mandatory)

| Standard | Value |
|----------|-------|
| **Target Framework** | .NET 10 (`net10.0`) |
| **Testing Framework** | xUnit + Moq (NOT NUnit/NSubstitute) |
| **Nullable** | Enabled |
| **Implicit Usings** | Enabled |
| **LangVersion** | Latest |

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `CustomerService` |
| Interfaces | I + PascalCase | `ICustomerRepository` |
| Methods | PascalCase | `GetCustomerById` |
| Properties | PascalCase | `FirstName` |
| Fields (private) | _camelCase | `_dbContext` |
| Parameters | camelCase | `customerId` |
| Local variables | camelCase | `customer` |

### DbContext Naming

**✅ Correct:**
```csharp
private readonly PocketWorkDbContext _context;
// or
private readonly PocketWorkDbContext _dbContext;
```

**❌ Wrong:**
```csharp
private readonly PocketWorkDbContext _db;  // Too generic
```

### Namespace Convention

**Folders:** NO `Olbrasoft.` prefix  
**Namespaces:** YES `Olbrasoft.` prefix

Example:
- **Folder:** `PocketWork.EntityFrameworkCore/`
- **Namespace:** `Olbrasoft.PocketWork.EntityFrameworkCore`

Set via `.csproj`:
```xml
<RootNamespace>Olbrasoft.PocketWork.{ProjectName}</RootNamespace>
```

### Test Naming

**Test classes**: `{SourceClass}Tests`  
**Test methods**: `[Method]_[Scenario]_[Expected]`

Examples:
- `GetCustomer_ValidId_ReturnsCustomer`
- `CreateOrder_EmptyCart_ThrowsException`

---

## Important Paths

- **Source:** `src/`
- **Tests:** `test/`
- **Solution:** `PocketWork.sln`
- **Database:** `pocketwork.db` (SQLite, created on first run)

---

## Secrets Management

### Local Development

**NEVER commit secrets to Git!**

Use .NET User Secrets:
```bash
dotnet user-secrets init --project src/PocketWork.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=pocketwork.db"
```

### Environment Variables

For production deployment:
```bash
export ConnectionStrings__DefaultConnection="Data Source=/path/to/pocketwork.db"
```

---

## Known Architectural Issues (PoC)

This PoC has **intentional simplifications** that should be fixed for production:

1. **❌ DbContext naming:** Should be `PocketWorkDbContext` not `_context`
2. **❌ Entities separate from DbContext:** Should be in same project
3. **❌ API → MVC communication:** Unnecessary HTTP overhead for same-server services
4. **❌ No DTOs:** API exposes entities directly
5. **❌ No Repository Pattern:** Direct DbContext usage in controllers
6. **❌ No Business Layer:** Business logic in controllers

See `README.md` for detailed explanation and recommended fixes.

---

## Deployment

**This PoC is not production-ready!**

For local testing only:
```bash
dotnet publish src/PocketWork.Api -c Release -o ./publish/api
dotnet publish src/PocketWork.Mvc -c Release -o ./publish/mvc
```

---

## Related Documentation

- **Engineering Handbook:** `~/GitHub/Olbrasoft/engineering-handbook/`
- **PoC Architecture Document:** `~/Dokumenty/ProofOfConcepts/PocketWork.md`
- **Git Workflow:** `~/GitHub/Olbrasoft/engineering-handbook/development-guidelines/workflow/git-workflow-workflow.md`
- **Testing Guide:** `~/GitHub/Olbrasoft/engineering-handbook/development-guidelines/dotnet/testing/unit-tests-testing.md`

---

## Contact

**Organization:** Olbrasoft  
**Repository:** https://github.com/Olbrasoft/PocketWork
