
using FluentMigrator;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Migrations.NHibernate;


[Migration(202404053)]
public class ProductMigration : Migration
{
  public override void Up()
  {
    _ = Create.Table(TableNames.Products)
        .WithColumn("id").AsGuid().PrimaryKey()
        .WithColumn("name").AsString(100).NotNullable()
        .WithColumn("price").AsDouble().Nullable();
  }

  public override void Down()
  {
    _ = Delete.Table(TableNames.Products);
  }
}
