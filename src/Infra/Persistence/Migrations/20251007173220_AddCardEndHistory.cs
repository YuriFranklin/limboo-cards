using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimbooCards.Infra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCardEndHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnd",
                table: "PlannerBuckets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHistory",
                table: "PlannerBuckets",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnd",
                table: "PlannerBuckets");

            migrationBuilder.DropColumn(
                name: "IsHistory",
                table: "PlannerBuckets");
        }
    }
}
