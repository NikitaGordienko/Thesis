using Microsoft.EntityFrameworkCore.Migrations;

namespace Thesis.Migrations
{
    public partial class SuggestedInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SuggestedInfo",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Terrain",
                table: "SuggestedInfo",
                newName: "TerrainId");

            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "SuggestedInfo",
                newName: "PhotoId");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "SuggestedInfo",
                newName: "DistrictId");

            migrationBuilder.AlterColumn<string>(
                name: "TypeId",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TerrainId",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhotoId",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DistrictId",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedInfo_DistrictId",
                table: "SuggestedInfo",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedInfo_PhotoId",
                table: "SuggestedInfo",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedInfo_TerrainId",
                table: "SuggestedInfo",
                column: "TerrainId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedInfo_TypeId",
                table: "SuggestedInfo",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestedInfo_Districts_DistrictId",
                table: "SuggestedInfo",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestedInfo_Files_PhotoId",
                table: "SuggestedInfo",
                column: "PhotoId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestedInfo_Terrains_TerrainId",
                table: "SuggestedInfo",
                column: "TerrainId",
                principalTable: "Terrains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestedInfo_ObjectTypes_TypeId",
                table: "SuggestedInfo",
                column: "TypeId",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuggestedInfo_Districts_DistrictId",
                table: "SuggestedInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_SuggestedInfo_Files_PhotoId",
                table: "SuggestedInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_SuggestedInfo_Terrains_TerrainId",
                table: "SuggestedInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_SuggestedInfo_ObjectTypes_TypeId",
                table: "SuggestedInfo");

            migrationBuilder.DropIndex(
                name: "IX_SuggestedInfo_DistrictId",
                table: "SuggestedInfo");

            migrationBuilder.DropIndex(
                name: "IX_SuggestedInfo_PhotoId",
                table: "SuggestedInfo");

            migrationBuilder.DropIndex(
                name: "IX_SuggestedInfo_TerrainId",
                table: "SuggestedInfo");

            migrationBuilder.DropIndex(
                name: "IX_SuggestedInfo_TypeId",
                table: "SuggestedInfo");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "SuggestedInfo",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TerrainId",
                table: "SuggestedInfo",
                newName: "Terrain");

            migrationBuilder.RenameColumn(
                name: "PhotoId",
                table: "SuggestedInfo",
                newName: "Photo");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "SuggestedInfo",
                newName: "District");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Terrain",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "SuggestedInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
