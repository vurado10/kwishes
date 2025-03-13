using System;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KWishes.Core.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role", "user,moderator,admin")
                .Annotation("Npgsql:Enum:vote_type", "wish_vote,comment_vote")
                .Annotation("Npgsql:Enum:wish_status", "moderation,in_process,rejected,completed");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    second_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<Role>(type: "role", nullable: false),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    google_name_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_email_1",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_google_name_id_1",
                table: "users",
                column: "google_name_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_username_1",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
