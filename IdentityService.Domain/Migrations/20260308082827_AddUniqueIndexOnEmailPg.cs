﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnEmailPg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop index if it exists (handles both fresh and existing databases)
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Users_Email\";");

            // Create unique index with PostgreSQL syntax
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the unique index
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            // Recreate non-unique index (if needed for rollback)
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");
        }
    }
}
