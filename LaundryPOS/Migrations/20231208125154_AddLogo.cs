﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaundryPOS.Migrations
{
    /// <inheritdoc />
    public partial class AddLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "AppSettings");
        }
    }
}