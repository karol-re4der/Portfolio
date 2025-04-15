using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class Sectioncoverphoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionCoverId",
                table: "Section",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Section_SectionCoverId",
                table: "Section",
                column: "SectionCoverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Photo_SectionCoverId",
                table: "Section",
                column: "SectionCoverId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_Photo_SectionCoverId",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Section_SectionCoverId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "SectionCoverId",
                table: "Section");
        }
    }
}
