using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagementAPI.Migrations
{
    public partial class validateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestTypes_TypeId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_TypeId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "TypeId",
                table: "Requests",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TypeId",
                table: "Requests",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestTypes_TypeId",
                table: "Requests",
                column: "TypeId",
                principalTable: "RequestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
