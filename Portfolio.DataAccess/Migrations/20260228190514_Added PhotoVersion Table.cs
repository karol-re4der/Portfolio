using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class AddedPhotoVersionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Photo_OriginalPhotoId",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_OriginalPhotoId",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "OriginalPhotoId",
                table: "Photo");

            migrationBuilder.CreateTable(
                name: "PhotoVersion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    IsOriginal = table.Column<bool>(type: "bit", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoVersion_Photo_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoVersion_PhotoId",
                table: "PhotoVersion",
                column: "PhotoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoVersion");

            migrationBuilder.AddColumn<int>(
                name: "OriginalPhotoId",
                table: "Photo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photo_OriginalPhotoId",
                table: "Photo",
                column: "OriginalPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Photo_OriginalPhotoId",
                table: "Photo",
                column: "OriginalPhotoId",
                principalTable: "Photo",
                principalColumn: "Id");
        }
    }
}
