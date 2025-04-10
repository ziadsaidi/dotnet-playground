using FluentNHibernate.Mapping;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.NH.Configurations;

public sealed class CustomerMapping : ClassMap<Customer>
{
  public CustomerMapping()
  {
    Table(TableNames.Customers);

    _ = Id(x => x.Id)
        .Column("id")
        .GeneratedBy.GuidComb()
        .CustomSqlType("uuid")
        .Not.Nullable();


    _ = HasMany(x => x.Sales)
        .Inverse()
        .Cascade.All()
        .KeyColumn("customer_id");
  }
}
