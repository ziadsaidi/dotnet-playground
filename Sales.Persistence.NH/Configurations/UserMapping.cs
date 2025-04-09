using FluentNHibernate.Mapping;
using Sales.Common.Constants;
using Sales.Domain.Entities;

namespace Sales.Persistence.NH.Configurations;

public sealed class UserMapping : ClassMap<User>
{
  public UserMapping()
  {
    Table(TableNames.Users);

    _ = Id(x => x.Id)
        .Column("id")
        .GeneratedBy.GuidComb()
        .CustomSqlType("uuid")
        .Not.Nullable();

    _ = Map(x => x.Email)
        .Column("email")
        .Not.Nullable()
        .Length(100)
        .Unique();

    _ = Map(x => x.Username)
        .Column("username")
        .Not.Nullable()
        .Length(100)
        .Unique();

    _ = Map(x => x.PasswordHash)
      .Column("password_hash")
      .Not.Nullable()
      .CustomSqlType("text");
  }
}
