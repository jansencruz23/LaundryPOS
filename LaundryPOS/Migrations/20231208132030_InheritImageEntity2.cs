using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaundryPOS.Migrations
{
    /// <inheritdoc />
    public partial class InheritImageEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "AppSettings",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "AppSettingsId",
                table: "AppSettings",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "AppSettings",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AppSettings",
                newName: "AppSettingsId");
        }
    }
}
