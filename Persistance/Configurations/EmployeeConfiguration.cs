using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Configurations;
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
  public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
  {
    builder.HasKey(e => e.Id)
           .HasName("employee_id");
    builder.Property(e => e.Id)
           .ValueGeneratedNever()
           .HasColumnName("id");
    builder.Property(e => e.Name)
           .IsRequired()
           .HasColumnName("name");
    builder.ToTable("employees");
  }
}
