using System;
using KWishes.Core.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KWishes.Core.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<VoteType>(type: "vote_type", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_votes", x => new { x.creator_id, x.type, x.entity_id });
                    table.ForeignKey(
                        name: "fk_votes_users_creator_temp_id1",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "votes");
        }
    }
}