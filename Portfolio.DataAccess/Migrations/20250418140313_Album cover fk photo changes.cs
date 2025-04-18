using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class Albumcoverfkphotochanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Photo_CoverPhotoId",
                table: "Album");

            migrationBuilder.AlterColumn<int>(
                name: "CoverPhotoId",
                table: "Album",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Photo_CoverPhotoId",
                table: "Album",
                column: "CoverPhotoId",
                principalTable: "Photo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Photo_CoverPhotoId",
                table: "Album");

            migrationBuilder.AlterColumn<int>(
                name: "CoverPhotoId",
                table: "Album",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Photo_CoverPhotoId",
                table: "Album",
                column: "CoverPhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
