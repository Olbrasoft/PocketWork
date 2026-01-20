# PocketWork - Proof of Concept

## Přehled projektu

PocketWork je webová aplikace pro správu objednávek a zákazníků s vícevrstvou architekturou.

## Architektura řešení

### Vrstvená architektura (podle engineering-handbook standardů)

```
PocketWork/
├── src/                            # Zdrojové projekty
│   ├── PocketWork.EntityFrameworkCore/   # Datová vrstva (Entities, EF Core, DbContext)
│   ├── PocketWork.Repositories/          # Repository vrstva (DTOs, Repositories)
│   ├── PocketWork.Api/                 # REST API (používá Repositories)
│   ├── PocketWork.Mvc/                 # Webová aplikace (MVC, používá Repositories)
│   └── PocketWork.Desktop/             # Desktop aplikace (Avalonia, používá API)
├── test/                           # Testovací projekty
│   ├── PocketWork.EntityFrameworkCore.Tests/
│   ├── PocketWork.Repositories.Tests/
│   ├── PocketWork.Api.Tests/
│   ├── PocketWork.Mvc.Tests/
│   └── PocketWork.Desktop.Tests/
├── .github/workflows/              # CI/CD
├── deploy/                         # Deployment skripty (optional)
├── .gitignore
├── LICENSE
├── README.md
├── AGENTS.md
└── PocketWork.sln
```

---

## 1. PocketWork.EntityFrameworkCore (Datová vrstva)

**Účel:** Datová vrstva obsahující entity, DbContext, konfigurace a repository pattern.

**Umístění:** `src/PocketWork.EntityFrameworkCore/`

**Technologie:**
- .NET 10
- Entity Framework Core 10
- **SQLite** (pro vývoj)

**Obsah:**
- **Entities/** - Modelové třídy: `User`, `Customer`, `ServiceType`, `Order`
- **Enums/** - Enumy: `JobType`, `OrderType`
- **PocketWorkDbContext.cs** - DbContext (v kořenovém adresáři)
- **Configurations/** - Fluent API konfigurace entit

**Namespace:** `Olbrasoft.PocketWork.EntityFrameworkCore`

### Struktura projektu

```
src/PocketWork.EntityFrameworkCore/
├── PocketWorkDbContext.cs          # DbContext v kořenu projektu
├── Entities/                       # Doménové entity
│   ├── User.cs
│   ├── Customer.cs
│   ├── ServiceType.cs
│   └── Order.cs
├── Enums/                          # Enumy
│   ├── JobType.cs
│   └── OrderType.cs
├── Configurations/                 # Fluent API konfigurace
│   ├── UserConfiguration.cs
│   ├── CustomerConfiguration.cs
│   ├── ServiceTypeConfiguration.cs
│   └── OrderConfiguration.cs
└── Migrations/
```

### .csproj konfigurace

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>Olbrasoft.PocketWork.EntityFrameworkCore</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.*" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.*" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.*" />
  </ItemGroup>
</Project>
```

### Modelové třídy (Entities)

#### User
```csharp
public sealed class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public JobType JobType { get; set; }
}
```

#### Customer
```csharp
public sealed class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
```

#### ServiceType
```csharp
public sealed class ServiceType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Price { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
}
```

#### Order
```csharp
public sealed class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public OrderType OrderType { get; set; }
    public DateTime OrderDate { get; set; }
    public TimeSpan OrderTime { get; set; }
    public TimeSpan ReservedTime { get; set; }
}
```

### Enumy

```csharp
public enum JobType
{
    None = 0,
    Worker = 1,
    Manager = 2,
    Admin = 3
}

public enum OrderType
{
    None = 0,
    Standard = 1,
    Express = 2,
    Subscription = 3
}
```

---

## 2. PocketWork.Api

**Účel:** RESTful API pro manipulaci s daty.

**Umístění:** `src/PocketWork.Api/`
├── DbContexts/
│   └── PocketWorkDbContext.cs
├── Configurations/
│   ├── UserConfiguration.cs
│   ├── CustomerConfiguration.cs
│   ├── ServiceTypeConfiguration.cs
│   └── OrderConfiguration.cs
├── Repositories/
│   ├── IRepository.cs
│   ├── Repository.cs
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── ICustomerRepository.cs
│   ├── CustomerRepository.cs
│   ├── IOrderRepository.cs
│   └── OrderRepository.cs
└── Migrations/
```

### .csproj konfigurace

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>Olbrasoft.PocketWork.EntityFrameworkCore</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.*" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.*" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.*" />
  </ItemGroup>
</Project>
```

---

## 2. PocketWork.Repositories (Repository vrstva + DTOs)

**Účel:** Abstrakční vrstva nad databází obsahující DTOs, repository pattern a business logiku přístupu k datům.

**Umístění:** `src/PocketWork.Repositories/`

**Technologie:**
- .NET 10
- Repository Pattern
- DTOs (Data Transfer Objects)

**Obsah:**
- **DTOs/** - Data Transfer Objects pro API a MVC
- **Repositories/** - Repository implementace
- **Interfaces/** - IRepository<T> a specifické repository rozhraní

**Namespace:** `Olbrasoft.PocketWork.Repositories`

### Struktura projektu

```
src/PocketWork.Repositories/
├── DTOs/                           # Data Transfer Objects
│   ├── Orders/
│   │   ├── CreateOrderDto.cs
│   │   ├── UpdateOrderDto.cs
│   │   └── OrderResponseDto.cs
│   ├── Customers/
│   │   ├── CreateCustomerDto.cs
│   │   ├── UpdateCustomerDto.cs
│   │   └── CustomerResponseDto.cs
│   └── Users/
│       ├── CreateUserDto.cs
│       └── UserResponseDto.cs
├── Repositories/                   # Repository implementace
│   ├── OrderRepository.cs
│   ├── CustomerRepository.cs
│   ├── UserRepository.cs
│   └── ServiceTypeRepository.cs
└── Interfaces/                     # Repository rozhraní
    ├── IRepository.cs
    ├── IOrderRepository.cs
    ├── ICustomerRepository.cs
    ├── IUserRepository.cs
    └── IServiceTypeRepository.cs
```

### .csproj konfigurace

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>Olbrasoft.PocketWork.Repositories</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\PocketWork.EntityFrameworkCore\PocketWork.EntityFrameworkCore.csproj" />
  </ItemGroup>
</Project>
```

### DTOs (Data Transfer Objects)

```csharp
// DTOs/Orders/CreateOrderDto.cs
namespace Olbrasoft.PocketWork.Repositories.DTOs.Orders;

public record CreateOrderDto
{
    public int CustomerId { get; init; }
    public OrderType OrderType { get; init; }
    public DateTime OrderDate { get; init; }
    public TimeSpan OrderTime { get; init; }
    public TimeSpan ReservedTime { get; init; }
}

// DTOs/Orders/UpdateOrderDto.cs
public record UpdateOrderDto
{
    public OrderType OrderType { get; init; }
    public DateTime OrderDate { get; init; }
    public TimeSpan OrderTime { get; init; }
    public TimeSpan ReservedTime { get; init; }
}

// DTOs/Orders/OrderResponseDto.cs
public record OrderResponseDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public string? CustomerName { get; init; }
    public OrderType OrderType { get; init; }
    public DateTime OrderDate { get; init; }
    public TimeSpan OrderTime { get; init; }
    public TimeSpan ReservedTime { get; init; }
}
```

### Repository Pattern

```csharp
// Interfaces/IRepository.cs
namespace Olbrasoft.PocketWork.Repositories.Interfaces;

public interface IRepository<TEntity, TCreateDto, TUpdateDto, TResponseDto> 
    where TEntity : class
{
    Task<TResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<TResponseDto>> GetAllAsync();
    Task<TResponseDto> CreateAsync(TCreateDto createDto);
    Task UpdateAsync(int id, TUpdateDto updateDto);
    Task DeleteAsync(int id);
}

// Interfaces/IOrderRepository.cs
public interface IOrderRepository : IRepository<Order, CreateOrderDto, UpdateOrderDto, OrderResponseDto>
{
    Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerIdAsync(int customerId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime from, DateTime to);
}
```

### Repository Implementation

```csharp
// Repositories/OrderRepository.cs
namespace Olbrasoft.PocketWork.Repositories.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PocketWorkDbContext _context;

    public OrderRepository(PocketWorkDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order == null ? null : MapToResponseDto(order);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .ToListAsync();

        return orders.Select(MapToResponseDto);
    }

    public async Task<OrderResponseDto> CreateAsync(CreateOrderDto createDto)
    {
        var order = new Order
        {
            CustomerId = createDto.CustomerId,
            OrderType = createDto.OrderType,
            OrderDate = createDto.OrderDate,
            OrderTime = createDto.OrderTime,
            ReservedTime = createDto.ReservedTime
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Reload s Customer
        await _context.Entry(order).Reference(o => o.Customer).LoadAsync();

        return MapToResponseDto(order);
    }

    public async Task UpdateAsync(int id, UpdateOrderDto updateDto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with id {id} not found");
        }

        order.OrderType = updateDto.OrderType;
        order.OrderDate = updateDto.OrderDate;
        order.OrderTime = updateDto.OrderTime;
        order.ReservedTime = updateDto.ReservedTime;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerIdAsync(int customerId)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();

        return orders.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime from, DateTime to)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Where(o => o.OrderDate >= from && o.OrderDate <= to)
            .ToListAsync();

        return orders.Select(MapToResponseDto);
    }

    private static OrderResponseDto MapToResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer != null 
                ? $"{order.Customer.Name} {order.Customer.Surname}" 
                : null,
            OrderType = order.OrderType,
            OrderDate = order.OrderDate,
            OrderTime = order.OrderTime,
            ReservedTime = order.ReservedTime
        };
    }
}
```

---

## 3. PocketWork.Api

**Účel:** RESTful API pro manipulaci s daty (používá Repository vrstvu).

**Umístění:** `src/PocketWork.Api/`

**Technologie:**
- ASP.NET Core Web API
- Controller-based API
- Swagger/OpenAPI dokumentace

### Struktura

```
src/PocketWork.Api/
├── Controllers/
│   ├── UsersController.cs
│   ├── CustomersController.cs
│   ├── ServiceTypesController.cs
│   └── OrdersController.cs
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
├── Program.cs
└── appsettings.json
```

### .csproj konfigurace

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>Olbrasoft.PocketWork.Api</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.*" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.*" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\PocketWork.Repositories\PocketWork.Repositories.csproj" />
  </ItemGroup>
</Project>
```

### Entity Configurations (Fluent API)

```csharp
// Configurations/OrderConfiguration.cs
namespace Olbrasoft.PocketWork.EntityFrameworkCore.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.OrderDate)
            .IsRequired();
        
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(o => o.OrderType)
            .HasConversion<int>();
    }
}
```

---

## ✅ Příklad controlleru (používá Repository vrstvu)

```csharp
namespace Olbrasoft.PocketWork.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto createDto)
    {
        try
        {
            var created = await _orderRepository.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetOrder), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDto updateDto)
    {
        try
        {
            await _orderRepository.UpdateAsync(id, updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersByCustomer(int customerId)
    {
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
        return Ok(orders);
    }

    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersByDateRange(
        [FromQuery] DateTime from, 
        [FromQuery] DateTime to)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(from, to);
        return Ok(orders);
    }
}
```

### Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database - SQLite pro vývoj
builder.Services.AddDbContext<PocketWorkDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

---

## 3. PocketWork.Mvc

**Účel:** Webová aplikace pro uživatelské rozhraní.

**Umístění:** `src/PocketWork.Mvc/`

**Technologie:**
- ASP.NET Core MVC
- Razor Views
- Bootstrap 5
- JavaScript (vanilla nebo framework dle preference)

### Struktura

```
src/PocketWork.Mvc/
├── Controllers/
│   ├── HomeController.cs
│   ├── OrdersController.cs
│   ├── CustomersController.cs
│   └── UsersController.cs
├── Views/
│   ├── Home/
│   │   └── Index.cshtml
│   ├── Orders/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Details.cshtml
│   ├── Customers/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _ValidationScriptsPartial.cshtml
├── Services/
│   ├── IApiClient.cs
│   └── ApiClient.cs
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
├── Program.cs
└── appsettings.json
```

### .csproj konfigurace

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>Olbrasoft.PocketWork.Mvc</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\PocketWork.EntityFrameworkCore\PocketWork.EntityFrameworkCore.csproj" />
  </ItemGroup>
</Project>
```

### Příklad controlleru (používá entity přímo)

```csharp
namespace Olbrasoft.PocketWork.Mvc.Controllers;

public class OrdersController : Controller
{
    private readonly IApiClient _apiClient;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IApiClient apiClient, ILogger<OrdersController> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _apiClient.GetOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _apiClient.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Order order)
    {
        if (!ModelState.IsValid)
        {
            return View(order);
        }

        await _apiClient.CreateOrderAsync(order);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var order = await _apiClient.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Order order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(order);
        }

        await _apiClient.UpdateOrderAsync(id, order);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _apiClient.DeleteOrderAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
```

### API Client (používá entity přímo)

```csharp
namespace Olbrasoft.PocketWork.Mvc.Services;

public interface IApiClient
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderAsync(int id, Order order);
    Task DeleteOrderAsync(int id);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;

    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var response = await _httpClient.GetAsync("/api/orders");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>() 
            ?? Enumerable.Empty<Order>();
    }

    public async Task<Order?> GetOrderAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/orders/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/orders", order);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Order>() 
            ?? throw new InvalidOperationException("Failed to deserialize created order");
    }

    public async Task UpdateOrderAsync(int id, Order order)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/orders/{id}", order);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/orders/{id}");
        response.EnsureSuccessStatusCode();
    }
}
```

### Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// ⚠️ ŠPATNĚ: MVC NEmá komunikovat s API!
// MVC by měl používat DbContext přímo
// Toto je jen pro demonstraci - v produkci NEPOUŽÍVAT!
builder.Services.AddHttpClient<IApiClient, ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7001");
});

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

**⚠️ POZNÁMKA:** V tomto PoC MVC komunikuje s API, což je **špatná praxe**! Správně by MVC měl používat `DbContext` přímo (viz níže správná architektura).

---

## 4. PocketWork.Desktop (Tlustý klient - Avalonia UI)

**Účel:** Cross-platform desktopová aplikace komunikující s API.

**Umístění:** `src/PocketWork.Desktop/`

**Technologie:**
- Avalonia UI (cross-platform GUI framework)
- MVVM pattern
- HTTP komunikace s API
- **Žádná lokální databáze, žádný offline režim**

### Struktura

```
src/PocketWork.Desktop/
├── Program.cs
├── App.axaml
├── Views/
│   ├── MainWindow.axaml
│   ├── OrdersView.axaml
│   ├── CustomersView.axaml
│   └── OrderDetailsView.axaml
├── ViewModels/
│   ├── MainWindowViewModel.cs
│   ├── OrdersViewModel.cs
│   ├── CustomersViewModel.cs
│   └── OrderDetailsViewModel.cs
├── Services/
│   ├── IApiClient.cs
│   └── ApiClient.cs              # HTTP komunikace s PocketWork.Api
├── Models/                       # Pokud potřebujeme lokální modely
└── Assets/
```

### .csproj konfigurace

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <OutputType>WinExe</OutputType>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <RootNamespace>Olbrasoft.PocketWork.Desktop</RootNamespace>
    <AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.2.*" />
    <PackageReference Include="Avalonia.Desktop" Version="11.2.*" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.2.*" />
    <PackageReference Include="Avalonia.ReactiveUI" Version="11.2.*" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\PocketWork.EntityFrameworkCore\PocketWork.EntityFrameworkCore.csproj" />
  </ItemGroup>
</Project>
```

### ApiClient (stejný princip jako MVC, ale SPRÁVNÉ použití)

```csharp
namespace Olbrasoft.PocketWork.Desktop.Services;

public interface IApiClient
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderAsync(int id, Order order);
    Task DeleteOrderAsync(int id);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var response = await _httpClient.GetAsync("/api/orders");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>() 
            ?? Enumerable.Empty<Order>();
    }

    // ... ostatní metody stejné jako v MVC
}
```

### ViewModel příklad (MVVM pattern)

```csharp
namespace Olbrasoft.PocketWork.Desktop.ViewModels;

public class OrdersViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private ObservableCollection<Order> _orders;
    private Order? _selectedOrder;

    public OrdersViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _orders = new ObservableCollection<Order>();
        
        LoadOrdersCommand = ReactiveCommand.CreateFromTask(LoadOrdersAsync);
        DeleteOrderCommand = ReactiveCommand.CreateFromTask<int>(DeleteOrderAsync);
    }

    public ObservableCollection<Order> Orders
    {
        get => _orders;
        set => this.RaiseAndSetIfChanged(ref _orders, value);
    }

    public Order? SelectedOrder
    {
        get => _selectedOrder;
        set => this.RaiseAndSetIfChanged(ref _selectedOrder, value);
    }

    public ICommand LoadOrdersCommand { get; }
    public ICommand DeleteOrderCommand { get; }

    private async Task LoadOrdersAsync()
    {
        var orders = await _apiClient.GetOrdersAsync();
        Orders = new ObservableCollection<Order>(orders);
    }

    private async Task DeleteOrderAsync(int id)
    {
        await _apiClient.DeleteOrderAsync(id);
        await LoadOrdersAsync();
    }
}
```

### Program.cs (Avalonia)

```csharp
namespace Olbrasoft.PocketWork.Desktop;

class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
```

### App.axaml.cs

```csharp
public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // DI setup
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            desktop.MainWindow = new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // HTTP Client pro API
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7001");
        });

        // ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<OrdersViewModel>();
        services.AddTransient<CustomersViewModel>();
    }
}
```

---

## ✅ Správná komunikační architektura

```
┌─────────────────┐
│   MVC Web App   │
│  (Server-side)  │
└────────┬────────┘
         │
         │ ✅ DIRECT DbContext access
         ▼
┌─────────────────┐
│ EntityFramework │
│  + SQLite DB    │
└─────────────────┘
         ▲
         │ ✅ DIRECT DbContext access
         │
┌────────┴────────┐
│   API Server    │
└────────┬────────┘
         │
         │ ✅ HTTP/REST (SPRÁVNÉ použití)
         ▼
┌─────────────────┐
│ Desktop Client  │
│   (Avalonia)    │
└─────────────────┘
```

### ✅ Správný komunikační tok pro MVC:

```
[Browser]
   ↓ HTTP Request
[PocketWork.Mvc Controllers]
   ↓ Repository call
[PocketWork.Repositories]
   ↓ DbContext access
[PocketWork.EntityFrameworkCore]
   ↓ SQL Query
[SQLite Database]
```

### ✅ Správný komunikační tok pro Desktop:

```
[Desktop App (Avalonia)]
   ↓ HTTP Request (✅ SPRÁVNĚ - externí klient)
[PocketWork.Api]
   ↓ Repository call
[PocketWork.Repositories]
   ↓ DbContext access
[PocketWork.EntityFrameworkCore]
   ↓ SQL Query
[SQLite Database]
```

---

## Závislosti mezi projekty

```
PocketWork.EntityFrameworkCore  ← Entities, DbContext, Configurations
         ↑
         │
PocketWork.Repositories  ← DTOs, Repository Pattern
         ↑
         ├─→ PocketWork.Api (používá Repositories)
         └─→ PocketWork.Mvc (používá Repositories)

PocketWork.Desktop → (komunikuje s API přes HTTP) ← ✅ SPRÁVNĚ pro tlustého klienta
```

---

## Konfigurace connectionStrings

### PocketWork.Api - appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=pocketwork.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Poznámka:** Pro produkci lze snadno přepnout na PostgreSQL nebo SQL Server změnou connection stringu a provider balíčku.

### PocketWork.Mvc - appsettings.json

**⚠️ ŠPATNĚ (jen v PoC):**
```json
{
  "ApiBaseUrl": "https://localhost:7001",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**✅ SPRÁVNĚ pro produkci:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=pocketwork.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### PocketWork.Desktop - appsettings.json

```json
{
  "ApiBaseUrl": "https://localhost:7001",
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## Další rozšíření (budoucnost)

- **Autentizace a autorizace:** ASP.NET Core Identity, JWT tokeny
- **Validace:** FluentValidation
- **Mapování:** AutoMapper
- **CQRS:** MediatR pattern
- **Testování:** xUnit, Moq
- **Caching:** Redis, MemoryCache
- **Message Queue:** RabbitMQ, Azure Service Bus
- **Monitoring:** Serilog, Application Insights

---

## Vytvoření projektů (CLI příkazy)

```bash
# Vytvoření adresářů podle standardů
mkdir -p PocketWork/{src,test}
cd PocketWork

# Vytvoření solution
dotnet new sln -n PocketWork

# === SOURCE PROJEKTY V src/ ===

# Data layer (Entity Framework Core)
dotnet new classlib -o src/PocketWork.EntityFrameworkCore
dotnet sln add src/PocketWork.EntityFrameworkCore/PocketWork.EntityFrameworkCore.csproj

# Repository layer (DTOs + Repositories)
dotnet new classlib -o src/PocketWork.Repositories
dotnet sln add src/PocketWork.Repositories/PocketWork.Repositories.csproj

# API
dotnet new webapi -o src/PocketWork.Api
dotnet sln add src/PocketWork.Api/PocketWork.Api.csproj

# MVC
dotnet new mvc -o src/PocketWork.Mvc
dotnet sln add src/PocketWork.Mvc/PocketWork.Mvc.csproj

# Desktop (Avalonia)
dotnet new install Avalonia.Templates
dotnet new avalonia.mvvm -o src/PocketWork.Desktop
dotnet sln add src/PocketWork.Desktop/PocketWork.Desktop.csproj

# === TESTOVACÍ PROJEKTY V test/ ===

# Test projekty (každý src projekt = odpovídající test projekt)
dotnet new xunit -o test/PocketWork.EntityFrameworkCore.Tests
dotnet sln add test/PocketWork.EntityFrameworkCore.Tests/PocketWork.EntityFrameworkCore.Tests.csproj

dotnet new xunit -o test/PocketWork.Repositories.Tests
dotnet sln add test/PocketWork.Repositories.Tests/PocketWork.Repositories.Tests.csproj

dotnet new xunit -o test/PocketWork.Api.Tests
dotnet sln add test/PocketWork.Api.Tests/PocketWork.Api.Tests.csproj

dotnet new xunit -o test/PocketWork.Mvc.Tests
dotnet sln add test/PocketWork.Mvc.Tests/PocketWork.Mvc.Tests.csproj

dotnet new xunit -o test/PocketWork.Desktop.Tests
dotnet sln add test/PocketWork.Desktop.Tests/PocketWork.Desktop.Tests.csproj

# === PŘIDÁNÍ PROJECT REFERENCÍ ===

# Source dependencies
dotnet add src/PocketWork.Repositories/PocketWork.Repositories.csproj reference src/PocketWork.EntityFrameworkCore/PocketWork.EntityFrameworkCore.csproj
dotnet add src/PocketWork.Api/PocketWork.Api.csproj reference src/PocketWork.Repositories/PocketWork.Repositories.csproj
dotnet add src/PocketWork.Mvc/PocketWork.Mvc.csproj reference src/PocketWork.Repositories/PocketWork.Repositories.csproj
dotnet add src/PocketWork.Desktop/PocketWork.Desktop.csproj reference src/PocketWork.EntityFrameworkCore/PocketWork.EntityFrameworkCore.csproj

# Test dependencies (test → src)
dotnet add test/PocketWork.EntityFrameworkCore.Tests/PocketWork.EntityFrameworkCore.Tests.csproj reference src/PocketWork.EntityFrameworkCore/PocketWork.EntityFrameworkCore.csproj
dotnet add test/PocketWork.Repositories.Tests/PocketWork.Repositories.Tests.csproj reference src/PocketWork.Repositories/PocketWork.Repositories.csproj
dotnet add test/PocketWork.Api.Tests/PocketWork.Api.Tests.csproj reference src/PocketWork.Api/PocketWork.Api.csproj
dotnet add test/PocketWork.Mvc.Tests/PocketWork.Mvc.Tests.csproj reference src/PocketWork.Mvc/PocketWork.Mvc.csproj
dotnet add test/PocketWork.Desktop.Tests/PocketWork.Desktop.Tests.csproj reference src/PocketWork.Desktop/PocketWork.Desktop.csproj

# === NUGET BALÍČKY ===

# Data layer - SQLite pro vývoj
dotnet add src/PocketWork.EntityFrameworkCore/PocketWork.EntityFrameworkCore.csproj package Microsoft.EntityFrameworkCore
dotnet add src/PocketWork.EntityFrameworkCore/PocketWork.EntityFrameworkCore.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/PocketWork.EntityFrameworkCore/PocketWork.EntityFrameworkCore.csproj package Microsoft.EntityFrameworkCore.Sqlite

# API
dotnet add src/PocketWork.Api/PocketWork.Api.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add src/PocketWork.Api/PocketWork.Api.csproj package Swashbuckle.AspNetCore

# Test projekty - xUnit + Moq
dotnet add test/PocketWork.EntityFrameworkCore.Tests/PocketWork.EntityFrameworkCore.Tests.csproj package Moq
dotnet add test/PocketWork.EntityFrameworkCore.Tests/PocketWork.EntityFrameworkCore.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory
dotnet add test/PocketWork.Repositories.Tests/PocketWork.Repositories.Tests.csproj package Moq
dotnet add test/PocketWork.Repositories.Tests/PocketWork.Repositories.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory
dotnet add test/PocketWork.Api.Tests/PocketWork.Api.Tests.csproj package Moq
dotnet add test/PocketWork.Mvc.Tests/PocketWork.Mvc.Tests.csproj package Moq
dotnet add test/PocketWork.Desktop.Tests/PocketWork.Desktop.Tests.csproj package Moq

# Avalonia packages pro Desktop
dotnet add src/PocketWork.Desktop/PocketWork.Desktop.csproj package Avalonia
dotnet add src/PocketWork.Desktop/PocketWork.Desktop.csproj package Avalonia.Desktop
dotnet add src/PocketWork.Desktop/PocketWork.Desktop.csproj package Avalonia.Themes.Fluent
dotnet add src/PocketWork.Desktop/PocketWork.Desktop.csproj package Avalonia.ReactiveUI

# === VYTVOŘENÍ DATABÁZE A MIGRACE ===

cd src/PocketWork.Api
dotnet ef migrations add InitialCreate --project ../PocketWork.EntityFrameworkCore
dotnet ef database update
cd ../..

# === SPUŠTĚNÍ TESTŮ ===

dotnet test
```

---

## Shrnutí

**PocketWork** je navržen jako vícevrstvá aplikace s následujícími komponentami:

1. **EntityFrameworkCore** - datová vrstva s entitami, DbContext a EF Core konfigurací
2. **Repositories** - repository vrstva s DTOs a repository pattern ✅
3. **Api** - RESTful API používající **Repositories** (pro Desktop klienty) ✅
4. **Mvc** - webová aplikace používající **Repositories** ✅
5. **Desktop** - Avalonia cross-platform aplikace komunikující s **API přes HTTP** ✅

## ✅ Co je v této architektuře SPRÁVNĚ:

- **Desktop → API:** Avalonia klient komunikuje s API přes HTTP ✅
  - Desktop je **externí klient**, běží na jiném počítači
  - HTTP komunikace je zde **správná a žádoucí**
  - Umožňuje distribuované nasazení

## ❌ Co je v tomto PoC ŠPATNĚ (pro demonstraci):

- **MVC → API:** V PoC MVC komunikuje s API přes HTTP ❌
  - MVC běží na **stejném serveru** jako databáze
  - HTTP volání je **zbytečná režie**
  - **Pro produkci:** MVC by měl používat DbContext přímo!

## Výhody správné architektury:

- **Jednoduchost** - přímé použití DbContext v MVC (server-side)
- **Výkon** - žádná HTTP režie pro server-side rendering
- **Snadnou údržbu** - každá vrstva má jasnou odpovědnost
- **Testovatelnost** - InMemory databáze pro testování
- **Škálovatelnost** - Desktop klienti mohou běžet kdekoli
- **Flexibilitu** - možnost přidat mobilní aplikaci nebo další klienty přes API

**Poznámka k architektuře PoC:**
- **Bez DTOs** - API i MVC pracují přímo s entitami pro jednoduchost
- **Bez Repository Pattern** - přímé použití DbContext v kontrolerech
- **MVC komunikuje s API** - jen pro demonstraci, v produkci NEPOUŽÍVAT!

**Pro produkci doporučeno:**
- **MVC → DbContext přímo** (odstranit HTTP komunikaci)
- Přidat DTOs pro oddělení API kontraktu od databázových entit
- Zvážit Repository/Unit of Work pattern pro lepší testovatelnost
- Implementovat Business vrstvu pro komplexní business logiku
- **Desktop → API** zachovat (správné použití)

---

**Vytvořeno:** 2026-01-20  
**Verze:** 1.0  
**Status:** Proof of Concept
