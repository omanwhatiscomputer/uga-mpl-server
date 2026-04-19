using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uga_mpl_server.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MeetupLocation_Latitude",
                table: "Products",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MeetupLocation_Longitude",
                table: "Products",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetupLocation_Latitude",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MeetupLocation_Longitude",
                table: "Products");
        }
    }
}
