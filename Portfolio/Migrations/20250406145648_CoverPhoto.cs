using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class CoverPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Album_CoverPhotoId",
                table: "Album",
                column: "CoverPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Photo_CoverPhotoId",
                table: "Album",
                column: "CoverPhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Photo_CoverPhotoId",
                table: "Album");

            migrationBuilder.DropIndex(
                name: "IX_Album_CoverPhotoId",
                table: "Album");
        }
    }
}
