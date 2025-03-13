using System;
using KWishes.Core.Domain;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KWishes.Core.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedWishesAndProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    logo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wishes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<WishStatus>(type: "wish_status", nullable: false),
                    comment_count = table.Column<int>(type: "integer", nullable: false),
                    vote_count = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    is_visible_for_users = table.Column<bool>(type: "boolean", nullable: false),
                    last_update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    product_id = table.Column<string>(type: "text", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wishes", x => x.id);
                    table.ForeignKey(
                        name: "fk_wishes_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_wishes_users_creator_temp_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_name_1",
                table: "products",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_creator_id_1",
                table: "wishes",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_id_1",
                table: "wishes",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wishes");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
