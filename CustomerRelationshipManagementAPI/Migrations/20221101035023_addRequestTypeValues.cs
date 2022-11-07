using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerRelationshipManagementAPI.Migrations
{
    public partial class addRequestTypeValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO RequestTypes VALUES (1,'Inquiry',1)");
            migrationBuilder.Sql("INSERT INTO RequestTypes VALUES (2,'Service Request',1)");
            migrationBuilder.Sql("INSERT INTO RequestTypes VALUES (3,'Complaint',1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
