﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaundryPOS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategoryImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicPath",
                table: "Categories",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Categories",
                newName: "PicPath");
        }
    }
}
