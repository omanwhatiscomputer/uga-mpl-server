using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uga_mpl_server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistAndSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<Guid>>(
                name: "SubscribedProductIds",
                table: "Users",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "WishlistedProductIds",
                table: "Users",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "SubscriberIds",
                table: "Products",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "WishlistedByUserIds",
                table: "Products",
                type: "uuid[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscribedProductIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WishlistedProductIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriberIds",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WishlistedByUserIds",
                table: "Products");
        }
    }
}
