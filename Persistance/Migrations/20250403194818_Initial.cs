using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
  /// <inheritdoc />
  public partial class Initial : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "customers",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            name = table.Column<string>(type: "text", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("customer_id", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "employees",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            name = table.Column<string>(type: "text", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("employee_id", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "products",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            name = table.Column<string>(type: "text", nullable: false),
            price = table.Column<double>(type: "double precision", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("product_id", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "sales",
          columns: table => new
          {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            customer_id = table.Column<Guid>(type: "uuid", nullable: true),
            employee_id = table.Column<Guid>(type: "uuid", nullable: true),
            product_id = table.Column<Guid>(type: "uuid", nullable: true),
            unit_price = table.Column<double>(type: "double precision", nullable: false),
            quantity = table.Column<int>(type: "integer", nullable: false),
            total_price = table.Column<double>(type: "double precision", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("sale_id", x => x.id);
            table.ForeignKey(
                      name: "sales_customer_fkey",
                      column: x => x.customer_id,
                      principalTable: "customers",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "sales_employee_fkey",
                      column: x => x.employee_id,
                      principalTable: "employees",
                      principalColumn: "id");
            table.ForeignKey(
                      name: "sales_product_fkey",
                      column: x => x.product_id,
                      principalTable: "products",
                      principalColumn: "id");
          });

      migrationBuilder.CreateIndex(
          name: "ix_sales_customer_id",
          table: "sales",
          column: "customer_id");

      migrationBuilder.CreateIndex(
          name: "ix_sales_employee_id",
          table: "sales",
          column: "employee_id");

      migrationBuilder.CreateIndex(
          name: "ix_sales_product_id",
          table: "sales",
          column: "product_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "sales");

      migrationBuilder.DropTable(
          name: "customers");

      migrationBuilder.DropTable(
          name: "employees");

      migrationBuilder.DropTable(
          name: "products");
    }
  }
}
