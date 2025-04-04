using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Configurations.EntityFramework
{
  public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
  {
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
    {
      _ = builder.HasKey(e => e.Id)
             .HasName("employee_id");
      _ = builder.Property(e => e.Id)
             .ValueGeneratedNever()
             .HasColumnName("id");
      _ = builder.Property(e => e.Name)
             .IsRequired()
             .HasColumnName("name");
      _ = builder.ToTable(TableNames.Employees);
    }
  }
}
