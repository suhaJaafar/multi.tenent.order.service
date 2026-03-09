using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddTenentNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenentName",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenentName",
                table: "Users");
        }
    }
}
