using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminNewsModule.Migrations
{
    public partial class updateNewsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CrateTime",
                table: "News",
                newName: "CreateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "News",
                newName: "CrateTime");
        }
    }
}
