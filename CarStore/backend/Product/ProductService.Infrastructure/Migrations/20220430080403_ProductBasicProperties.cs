using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Infrastructure.Migrations
{
    public partial class ProductBasicProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "products",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "cost",
                table: "products",
                newName: "price");

            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "fuel_type",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "has_installment",
                table: "products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "km_driven",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "made_in",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "model",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                table: "products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "seating_capacity",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "transmission",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "verified",
                table: "products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "brand",
                table: "products");

            migrationBuilder.DropColumn(
                name: "category",
                table: "products");

            migrationBuilder.DropColumn(
                name: "color",
                table: "products");

            migrationBuilder.DropColumn(
                name: "description",
                table: "products");

            migrationBuilder.DropColumn(
                name: "fuel_type",
                table: "products");

            migrationBuilder.DropColumn(
                name: "has_installment",
                table: "products");

            migrationBuilder.DropColumn(
                name: "km_driven",
                table: "products");

            migrationBuilder.DropColumn(
                name: "made_in",
                table: "products");

            migrationBuilder.DropColumn(
                name: "model",
                table: "products");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "products");

            migrationBuilder.DropColumn(
                name: "seating_capacity",
                table: "products");

            migrationBuilder.DropColumn(
                name: "transmission",
                table: "products");

            migrationBuilder.DropColumn(
                name: "verified",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "products",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "products",
                newName: "cost");
        }
    }
}
