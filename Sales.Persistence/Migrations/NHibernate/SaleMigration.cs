using FluentMigrator;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Migrations.NHibernate;

[Migration(202404054)]
public class SaleMigration : Migration
{
  public override void Up()
  {
    _ = Create.Table(TableNames.Sales)
        .WithColumn("id").AsGuid().PrimaryKey()
        .WithColumn("date").AsDateTime().NotNullable()
        .WithColumn("unit_price").AsDouble().NotNullable()
        .WithColumn("quantity").AsInt32().NotNullable()
        .WithColumn("total_price").AsDouble().NotNullable()
        .WithColumn("customer_id").AsGuid().Nullable()
        .WithColumn("employee_id").AsGuid().Nullable()
        .WithColumn("product_id").AsGuid().Nullable();

    // Create Foreign Key Constraints
    _ = Create.ForeignKey("FK_Sales_Customers")
        .FromTable(TableNames.Sales).ForeignColumn("customer_id")
        .ToTable(TableNames.Customers).PrimaryColumn("id");

    _ = Create.ForeignKey("FK_Sales_Employees")
        .FromTable(TableNames.Sales).ForeignColumn("employee_id")
        .ToTable(TableNames.Employees).PrimaryColumn("id");

    _ = Create.ForeignKey("FK_Sales_Products")
        .FromTable(TableNames.Sales).ForeignColumn("product_id")
        .ToTable(TableNames.Products).PrimaryColumn("id");
  }

  public override void Down()
  {
    _ = Delete.Table(TableNames.Sales);
  }
}
