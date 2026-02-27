using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class OriginalPhotorelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
