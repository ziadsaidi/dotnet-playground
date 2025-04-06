using FluentNHibernate.Mapping;
using Sales.Domain.Entities;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Configurations.NHibernate;

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

    _ = HasMany(x => x.Sales)
        .Inverse()
        .Cascade.All()
        .KeyColumn("product_id");
  }
}
