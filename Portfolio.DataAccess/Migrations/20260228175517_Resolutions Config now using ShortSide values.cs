using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class ResolutionsConfignowusingShortSidevalues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "ResolutionConfig");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "ResolutionConfig",
                newName: "ShortSide");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortSide",
                table: "ResolutionConfig",
                newName: "Width");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "ResolutionConfig",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
