
using FluentMigrator;
using Sales.Persistence.Constants;

namespace Sales.Persistence.NHibernateMigrations;

[Migration(20240405)]
public class EmployeeMigration : Migration
{
  public override void Up()
  {
    _ = Create.Table(TableNames.Employees)
        .WithColumn("id").AsGuid().PrimaryKey()
        .WithColumn("name").AsString(100).NotNullable();
  }

  public override void Down()
  {
    _ = Delete.Table(TableNames.Employees);
  }
}
