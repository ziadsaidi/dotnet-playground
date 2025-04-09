using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "sales",
                newName: "creation_date");

            migrationBuilder.AlterColumn<double>(
                name: "price",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    name = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "sales",
                newName: "date");

            migrationBuilder.AlterColumn<double>(
                name: "price",
                table: "products",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
