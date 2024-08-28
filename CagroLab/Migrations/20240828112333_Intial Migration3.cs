using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagroLab.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_Account_AccountId",
                table: "Package");

            migrationBuilder.DropIndex(
                name: "IX_Package_AccountId",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Package");

            migrationBuilder.CreateIndex(
                name: "IX_Package_Account_Id",
                table: "Package",
                column: "Account_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Account_Account_Id",
                table: "Package",
                column: "Account_Id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_Account_Account_Id",
                table: "Package");

            migrationBuilder.DropIndex(
                name: "IX_Package_Account_Id",
                table: "Package");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Package_AccountId",
                table: "Package",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Account_AccountId",
                table: "Package",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
