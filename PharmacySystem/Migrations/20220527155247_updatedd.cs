using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacySystem.Migrations
{
    public partial class updatedd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false),
                    Quantity = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageName = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Invoice_No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    CustName = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: true),
                    Type = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Employee_Username = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Invoice_No);
                    table.ForeignKey(
                        name: "FK_Invoice_Employee",
                        column: x => x.Employee_Username,
                        principalTable: "Employee",
                        principalColumn: "UserName");
                });

            migrationBuilder.CreateTable(
                name: "Item_Invoice",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Invoice", x => new { x.InvoiceID, x.ItemID });
                    table.ForeignKey(
                        name: "FK_Item_Invoice_Invoice",
                        column: x => x.InvoiceID,
                        principalTable: "Invoice",
                        principalColumn: "Invoice_No");
                    table.ForeignKey(
                        name: "FK_Item_Invoice_Item",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Employee_Username",
                table: "Invoice",
                column: "Employee_Username");

            migrationBuilder.CreateIndex(
                name: "nameUnique",
                table: "Item",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_Invoice_ItemID",
                table: "Item_Invoice",
                column: "ItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item_Invoice");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
