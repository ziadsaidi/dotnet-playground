using Microsoft.EntityFrameworkCore;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
  public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
  {
    _ = builder.HasKey(static e => e.Id)
           .HasName("employee_id");
    _ = builder.Property(static e => e.Id)
           .ValueGeneratedNever()
           .HasColumnName("id");
    _ = builder.Property(static e => e.Name)
           .IsRequired()
           .HasColumnName("name");
    _ = builder.ToTable(TableNames.Employees);
  }
}

