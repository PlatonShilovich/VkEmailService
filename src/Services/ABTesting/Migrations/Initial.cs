using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ABTesting.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "abtesting");

            migrationBuilder.CreateTable(
                name: "Experiments",
                schema: "abtesting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    TrafficSplit = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Variants",
                schema: "abtesting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ExperimentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variants_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalSchema: "abtesting",
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAssignments",
                schema: "abtesting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ExperimentId = table.Column<Guid>(nullable: false),
                    VariantId = table.Column<Guid>(nullable: false),
                    AssignedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssignments_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalSchema: "abtesting",
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAssignments_Variants_VariantId",
                        column: x => x.VariantId,
                        principalSchema: "abtesting",
                        principalTable: "Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_ExperimentId",
                schema: "abtesting",
                table: "UserAssignments",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_VariantId",
                schema: "abtesting",
                table: "UserAssignments",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_ExperimentId",
                schema: "abtesting",
                table: "Variants",
                column: "ExperimentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAssignments",
                schema: "abtesting");

            migrationBuilder.DropTable(
                name: "Variants",
                schema: "abtesting");

            migrationBuilder.DropTable(
                name: "Experiments",
                schema: "abtesting");
        }
    }
}
