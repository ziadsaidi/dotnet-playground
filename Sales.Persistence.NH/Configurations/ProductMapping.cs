using FluentNHibernate.Mapping;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.NH.Configurations;

public sealed class ProductMapping : ClassMap<Product>
{
  public ProductMapping()
  {
    Table(TableNames.Products);

    _ = Id(x => x.Id)
           .Column("id")
           .GeneratedBy.GuidComb()
           .CustomSqlType("uuid")
           .Not.Nullable();

    _ = Map(x => x.Name)
        .Column("name")
        .Not.Nullable();

    _ = Map(x => x.Price)
         .Column("price")
         .Nullable();

  }
}
