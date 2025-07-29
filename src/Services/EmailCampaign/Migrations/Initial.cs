using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmailCampaign.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "emailcampaign");

            migrationBuilder.CreateTable(
                name: "Campaigns",
                schema: "emailcampaign",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    SegmentId = table.Column<Guid>(nullable: true),
                    ExperimentId = table.Column<Guid>(nullable: true),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campaigns",
                schema: "emailcampaign");
        }
    }
}