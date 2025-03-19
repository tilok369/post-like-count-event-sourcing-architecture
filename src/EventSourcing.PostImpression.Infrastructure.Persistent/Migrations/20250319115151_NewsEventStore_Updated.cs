using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSourcing.PostImpression.Infrastructure.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class NewsEventStore_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                table: "NewsEventStores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedOn",
                table: "NewsEventStores",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processed",
                table: "NewsEventStores");

            migrationBuilder.DropColumn(
                name: "ProcessedOn",
                table: "NewsEventStores");
        }
    }
}
