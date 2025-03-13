using System;
using System.Collections.Generic;
using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KWishes.Core.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE wish_status RENAME VALUE 'moderation' TO 'moderating';");
            migrationBuilder.Sql("ALTER TYPE wish_status RENAME VALUE 'in_process' TO 'processing';");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    wish_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_role = table.Column<Role>(type: "role", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vote_count = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    files = table.Column<IReadOnlyList<Uri>>(type: "jsonb", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_comments_users_comment_creator_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_wishes_wish_id",
                        column: x => x.wish_id,
                        principalTable: "wishes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_comments_creator_id",
                table: "comments",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_wish_id",
                table: "comments",
                column: "wish_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.Sql("ALTER TYPE wish_status RENAME VALUE 'moderating' TO 'moderation';");
            migrationBuilder.Sql("ALTER TYPE wish_status RENAME VALUE 'processing' TO 'in_process';");
        }
    }
}
