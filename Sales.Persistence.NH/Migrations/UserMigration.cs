using FluentMigrator;
using Sales.Common.Constants;

namespace Sales.Persistence.NH.Migrations;

[Migration(202404058)]
public class UserMigration : Migration
{
  public override void Up()
  {
    _ = Create.Table(TableNames.Users)
        .WithColumn("id").AsGuid().PrimaryKey()
        .WithColumn("email").AsString(100).NotNullable()
        .WithColumn("username").AsString(100).NotNullable()
        .WithColumn("password_hash").AsString().NotNullable();

    // Unique index on email
    _ = Create.Index("IX_Users_Email")
        .OnTable(TableNames.Users)
        .OnColumn("email").Unique();

    // Unique index on username
    _ = Create.Index("IX_Users_Username")
        .OnTable(TableNames.Users)
        .OnColumn("username").Unique();
  }

  public override void Down()
  {
    _ = Delete.Index("IX_Users_Email").OnTable(TableNames.Users);
    _ = Delete.Index("IX_Users_Username").OnTable(TableNames.Users);

    _ = Delete.Table(TableNames.Users);
  }
}
