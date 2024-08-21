using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreeBilling.Web.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBills_Customers_CustomerId",
                table: "TimeBills");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeBills_Employees_EmployeeId",
                table: "TimeBills");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "TimeBills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "TimeBills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBills_Customers_CustomerId",
                table: "TimeBills",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBills_Employees_EmployeeId",
                table: "TimeBills",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBills_Customers_CustomerId",
                table: "TimeBills");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeBills_Employees_EmployeeId",
                table: "TimeBills");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "TimeBills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "TimeBills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBills_Customers_CustomerId",
                table: "TimeBills",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBills_Employees_EmployeeId",
                table: "TimeBills",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
