using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagroLab.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Account_Id",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account_Id",
                table: "Package");
        }
    }
}
