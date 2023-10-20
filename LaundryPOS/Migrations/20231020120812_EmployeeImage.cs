using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaundryPOS.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicPath",
                table: "Employees",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Employees",
                newName: "PicPath");
        }
    }
}
