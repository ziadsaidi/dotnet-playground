using Microsoft.EntityFrameworkCore;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.EF.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
  public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
  {
    _ = builder.HasKey(e => e.Id)
           .HasName("employee_id");

    _ = builder.Property(e => e.Id)
           .ValueGeneratedNever()
           .HasColumnName("id");

    _ = builder.Property(e => e.Salary)
              .IsRequired()
              .HasColumnName("salary");

    _ = builder.Property(e => e.Position)
                .HasConversion<string>()
                .HasColumnName("position");

    _ = builder.HasOne(e => e.User)
          .WithOne(c => c.Employee)
          .HasForeignKey<Employee>(e => e.UserId)
          .IsRequired(false)
          .OnDelete(DeleteBehavior.SetNull)
          .HasConstraintName("fk_Users_employees");


 

    _ = builder.ToTable(TableNames.Employees);
  }
}
