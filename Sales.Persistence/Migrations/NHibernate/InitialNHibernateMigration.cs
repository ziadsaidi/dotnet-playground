using FluentMigrator;
using Sales.Persistence.Constants;

namespace Sales.Persistence.NHibernateMigrations;

[Migration(20240404)]
public class InitialNHibernateMigration : Migration
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
