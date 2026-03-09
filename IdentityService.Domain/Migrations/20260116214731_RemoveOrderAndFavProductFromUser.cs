using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrderAndFavProductFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProductId",
                table: "Users",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Products_ProductId",
                table: "Users",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Products_ProductId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProductId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserFavoriteProducts",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteProducts", x => new { x.ProductId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteProducts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteProducts_UserId",
                table: "UserFavoriteProducts",
                column: "UserId");
        }
    }
}
