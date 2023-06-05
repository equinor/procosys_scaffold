using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Equinor.ProCoSys.PCS5.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IHaveGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_Oid",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Oid",
                table: "Persons");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Guid",
                table: "Persons",
                column: "Guid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_Guid",
                table: "Persons");

            migrationBuilder.AddColumn<Guid>(
                name: "Oid",
                table: "Persons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Oid",
                table: "Persons",
                column: "Oid",
                unique: true);
        }
    }
}
