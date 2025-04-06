using FluentMigrator;
using Sales.Persistence.Constants;

namespace Sales.Persistence.Migrations.NHibernate;

[Migration(202404052)]
public class CustomerMigration : Migration
{
  public override void Up()
  {
    _ = Create.Table(TableNames.Customers)
        .WithColumn("id").AsGuid().PrimaryKey()
        .WithColumn("name").AsString(100).NotNullable();
  }

  public override void Down()
  {
    _ = Delete.Table(TableNames.Customers);
  }
}
