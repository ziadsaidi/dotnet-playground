using FluentNHibernate.Mapping;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.NH.Configurations;

public sealed class EmployeeMapping : ClassMap<Employee>
{
  public EmployeeMapping()
  {
    Table(TableNames.Employees);

    _ = Id(static x => x.Id)
             .Column("id")
             .GeneratedBy.GuidComb()
              .CustomSqlType("uuid")
             .Not.Nullable();

    _ = Map(static x => x.Name)
        .Column("name")
        .Not.Nullable();

    _ = HasMany(static x => x.Sales)
        .Inverse()
        .Cascade.All()
        .KeyColumn("employee_id");
  }
}