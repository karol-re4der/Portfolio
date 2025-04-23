using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class sectioncoverid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_Photo_SectionCoverId",
                table: "Section");

            migrationBuilder.AlterColumn<int>(
                name: "SectionCoverId",
                table: "Section",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Photo_SectionCoverId",
                table: "Section",
                column: "SectionCoverId",
                principalTable: "Photo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_Photo_SectionCoverId",
                table: "Section");

            migrationBuilder.AlterColumn<int>(
                name: "SectionCoverId",
                table: "Section",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Photo_SectionCoverId",
                table: "Section",
                column: "SectionCoverId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
