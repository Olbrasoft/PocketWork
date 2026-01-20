using Microsoft.EntityFrameworkCore;
using Olbrasoft.PocketWork.EntityFrameworkCore.Entities;

namespace Olbrasoft.PocketWork.EntityFrameworkCore;

public class PocketWorkDbContext : DbContext
{
    public PocketWorkDbContext(DbContextOptions<PocketWorkDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PocketWorkDbContext).Assembly);
    }
}
