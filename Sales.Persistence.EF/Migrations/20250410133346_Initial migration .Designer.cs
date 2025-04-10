﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sales.Persistence.EF.Data.Configuration;

#nullable disable

namespace Sales.Persistence.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250410133346_Initial migration ")]
    partial class Initialmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Sales.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("address");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("customer_id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_customers_user_id");

                    b.ToTable("customers", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("position");

                    b.Property<double>("Salary")
                        .HasColumnType("double precision")
                        .HasColumnName("salary");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("employee_id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_employees_user_id");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Entities.InventoryTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("PerformedByEmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("performed_by_employee_id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("QuantityChange")
                        .HasColumnType("integer")
                        .HasColumnName("quantity_change");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("Id")
                        .HasName("pk_inventory_transaction");

                    b.HasIndex("PerformedByEmployeeId")
                        .IsUnique()
                        .HasDatabaseName("ix_inventory_transaction_performed_by_employee_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_inventory_transaction_product_id");

                    b.ToTable("inventory_transaction", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("stock_quantity");

                    b.HasKey("Id")
                        .HasName("product_id");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Entities.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("employee_id");

                    b.HasKey("Id")
                        .HasName("sale_id");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("ix_sales_customer_id");

                    b.HasIndex("EmployeeId")
                        .HasDatabaseName("ix_sales_employee_id");

                    b.ToTable("sales", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Entities.SaleLineItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<Guid>("SaleId")
                        .HasColumnType("uuid")
                        .HasColumnName("sale_id");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("unit_price");

                    b.HasKey("Id")
                        .HasName("pk_products_saleline_item");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_sale_line_item_product_id");

                    b.HasIndex("SaleId")
                        .HasDatabaseName("ix_sale_line_item_sale_id");

                    b.ToTable("sale_line_item", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("IX_Users_Email");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("IX_Users_Username");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Sales.Domain.Entities.Customer", b =>
                {
                    b.HasOne("Sales.Domain.Entities.User", "User")
                        .WithOne("Customer")
                        .HasForeignKey("Sales.Domain.Entities.Customer", "UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_Users_customers");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sales.Domain.Entities.Employee", b =>
                {
                    b.HasOne("Sales.Domain.Entities.User", "User")
                        .WithOne("Employee")
                        .HasForeignKey("Sales.Domain.Entities.Employee", "UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_Users_employees");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sales.Domain.Entities.InventoryTransaction", b =>
                {
                    b.HasOne("Sales.Domain.Entities.Employee", "PerformedBy")
                        .WithOne("InventoryTransaction")
                        .HasForeignKey("Sales.Domain.Entities.InventoryTransaction", "PerformedByEmployeeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_inventory_transactions_employee");

                    b.HasOne("Sales.Domain.Entities.Product", "Product")
                        .WithMany("InventoryTransactions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_inventory_transactions");

                    b.Navigation("PerformedBy");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Sales.Domain.Entities.Sale", b =>
                {
                    b.HasOne("Sales.Domain.Entities.Customer", "Customer")
                        .WithMany("Sales")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("sales_customer_fkey");

                    b.HasOne("Sales.Domain.Entities.Employee", "Employee")
                        .WithMany("Sales")
                        .HasForeignKey("EmployeeId")
                        .IsRequired()
                        .HasConstraintName("sales_employee_fkey");

                    b.Navigation("Customer");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Sales.Domain.Entities.SaleLineItem", b =>
                {
                    b.HasOne("Sales.Domain.Entities.Product", "Product")
                        .WithMany("SaleLineItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_sales_items");

                    b.HasOne("Sales.Domain.Entities.Sale", "Sale")
                        .WithMany("LineItems")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sale_line_item_sales_sale_id");

                    b.Navigation("Product");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("Sales.Domain.Entities.Customer", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Sales.Domain.Entities.Employee", b =>
                {
                    b.Navigation("InventoryTransaction")
                        .IsRequired();

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Sales.Domain.Entities.Product", b =>
                {
                    b.Navigation("InventoryTransactions");

                    b.Navigation("SaleLineItems");
                });

            modelBuilder.Entity("Sales.Domain.Entities.Sale", b =>
                {
                    b.Navigation("LineItems");
                });

            modelBuilder.Entity("Sales.Domain.Entities.User", b =>
                {
                    b.Navigation("Customer");

                    b.Navigation("Employee");
                });
#pragma warning restore 612, 618
        }
    }
}
