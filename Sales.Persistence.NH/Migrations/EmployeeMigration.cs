
using FluentMigrator;
using Sales.Common.Constants;

namespace Sales.Persistence.NH.Migrations;

[Migration(202404051)]
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
