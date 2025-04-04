using FluentNHibernate.Mapping;
using Sales.Domain.Entities;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Configurations.NHibernate
{
  public class EmployeeMapping : ClassMap<Employee>
  {
    public EmployeeMapping()
    {
      Table(TableNames.Employees);

      _ = Id(x => x.Id)
               .Column("id")
               .GeneratedBy.GuidComb()
                .CustomSqlType("uuid")
               .Not.Nullable();

      _ = Map(x => x.Name)
          .Column("name")
          .Not.Nullable();

      _ = HasMany(x => x.Sales)
          .Inverse()
          .Cascade.All()
          .KeyColumn("employee_id");
    }
  }
}