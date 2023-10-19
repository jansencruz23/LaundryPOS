using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaundryPOS.Migrations
{
    /// <inheritdoc />
    public partial class ImageByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicPath",
                table: "Items");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Items",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "PicPath",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
