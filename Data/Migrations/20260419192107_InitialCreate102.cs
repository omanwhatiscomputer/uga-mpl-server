using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uga_mpl_server.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpoPushToken",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpoPushToken",
                table: "Users");
        }
    }
}
