using FluentNHibernate.Mapping;
using Sales.Domain.Entities;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Configurations.NHibernate;

public sealed class SaleMapping : ClassMap<Sale>
{
  public SaleMapping()
  {
    Table(TableNames.Sales);

    _ = Id(x => x.Id)
           .Column("id")
           .GeneratedBy.GuidComb()
           .CustomSqlType("uuid")
           .Not.Nullable();

    _ = Map(x => x.UnitPrice)
        .Column("unit_price")
        .Not.Nullable();

    _ = Map(x => x.Quantity)
        .Column("quantity")
        .Not.Nullable();

    _ = Map(x => x.TotalPrice)
        .Column("total_price")
        .Not.Nullable();

    // Relationships
    _ = References(x => x.Customer)
        .Column("customer_id")
        .Not.Nullable()
        .Cascade.SaveUpdate()
        .ForeignKey("sales_customer_fkey");

    _ = References(x => x.Employee)
        .Column("employee_id")
        .Not.Nullable()
        .Cascade.SaveUpdate()
        .ForeignKey("sales_employee_fkey");

    _ = References(x => x.Product)
        .Column("product_id")
        .Not.Nullable()
        .Cascade.SaveUpdate()
        .ForeignKey("sales_product_fkey");
  }
}
