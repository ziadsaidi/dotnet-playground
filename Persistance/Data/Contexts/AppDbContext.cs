using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data.Contexts;
public class AppDbContext : DbContext, IApplicationDbContext
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
