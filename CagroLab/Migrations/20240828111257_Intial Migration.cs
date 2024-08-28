using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CagroLab.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lab",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lab_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lab_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lab_Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lab_Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Account_Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Full_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Last_Activity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Last_Login = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Main_Account = table.Column<bool>(type: "bit", nullable: false),
                    Lab_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Lab_Lab_Id",
                        column: x => x.Lab_Id,
                        principalTable: "Lab",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Package_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Package_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Lab_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Package_Lab_Lab_Id",
                        column: x => x.Lab_Id,
                        principalTable: "Lab",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Package_Id = table.Column<int>(type: "int", nullable: false),
                    Sample_Type_Id = table.Column<int>(type: "int", nullable: false),
                    Patient_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patient_Id = table.Column<int>(type: "int", nullable: false),
                    Patient_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Account_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sample_Account_Account_Id",
                        column: x => x.Account_Id,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sample_Package_Package_Id",
                        column: x => x.Package_Id,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Lab_Id",
                table: "Account",
                column: "Lab_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Package_AccountId",
                table: "Package",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_Lab_Id",
                table: "Package",
                column: "Lab_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_Account_Id",
                table: "Sample",
                column: "Account_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sample_Package_Id",
                table: "Sample",
                column: "Package_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Lab");
        }
    }
}
