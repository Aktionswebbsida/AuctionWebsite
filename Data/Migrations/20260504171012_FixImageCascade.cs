using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixImageCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Ads_AdID",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Ads_AdID",
                table: "Images",
                column: "AdID",
                principalTable: "Ads",
                principalColumn: "AdID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Ads_AdID",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Ads_AdID",
                table: "Images",
                column: "AdID",
                principalTable: "Ads",
                principalColumn: "AdID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
