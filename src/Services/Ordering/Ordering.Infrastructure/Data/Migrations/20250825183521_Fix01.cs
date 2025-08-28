using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Data.Migrations
{
	/// <inheritdoc />
	public partial class Fix01 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "LastModifiedDate",
				table: "Products",
				newName: "LastModified");

			migrationBuilder.RenameColumn(
				name: "CreatedDate",
				table: "Products",
				newName: "CreatedAt");

			migrationBuilder.RenameColumn(
				name: "LastModifiedDate",
				table: "Orders",
				newName: "LastModified");

			migrationBuilder.RenameColumn(
				name: "CreatedDate",
				table: "Orders",
				newName: "CreatedAt");

			migrationBuilder.RenameColumn(
				name: "LastModifiedDate",
				table: "OrderItems",
				newName: "LastModified");

			migrationBuilder.RenameColumn(
				name: "CreatedDate",
				table: "OrderItems",
				newName: "CreatedAt");

			migrationBuilder.RenameColumn(
				name: "LastModifiedDate",
				table: "Customers",
				newName: "LastModified");

			migrationBuilder.RenameColumn(
				name: "CreatedDate",
				table: "Customers",
				newName: "CreatedAt");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "LastModified",
				table: "Products",
				newName: "LastModifiedDate");

			migrationBuilder.RenameColumn(
				name: "CreatedAt",
				table: "Products",
				newName: "CreatedDate");

			migrationBuilder.RenameColumn(
				name: "LastModified",
				table: "Orders",
				newName: "LastModifiedDate");

			migrationBuilder.RenameColumn(
				name: "CreatedAt",
				table: "Orders",
				newName: "CreatedDate");

			migrationBuilder.RenameColumn(
				name: "LastModified",
				table: "OrderItems",
				newName: "LastModifiedDate");

			migrationBuilder.RenameColumn(
				name: "CreatedAt",
				table: "OrderItems",
				newName: "CreatedDate");

			migrationBuilder.RenameColumn(
				name: "LastModified",
				table: "Customers",
				newName: "LastModifiedDate");

			migrationBuilder.RenameColumn(
				name: "CreatedAt",
				table: "Customers",
				newName: "CreatedDate");
		}
	}
}
