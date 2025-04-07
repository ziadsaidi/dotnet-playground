using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Data.Configuration;

public class AppDbContext : DbContext
{
  public AppDbContext()
  {
  }

  public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }

  public virtual DbSet<Sale> Sales { get; set; }
  public virtual DbSet<Customer> Customers { get; set; }
  public virtual DbSet<Employee> Employees { get; set; }
  public virtual DbSet<Product> Products { get; set; }
}
