using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class Carouselphotonullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carousel_Photo_PhotoId",
                table: "Carousel");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "Carousel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Carousel_Photo_PhotoId",
                table: "Carousel",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carousel_Photo_PhotoId",
                table: "Carousel");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "Carousel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carousel_Photo_PhotoId",
                table: "Carousel",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
