using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SeaStore.Entities.Migrations
{
    public partial class _10262018_2_DBChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boats_BoatTypes_BoatTypeId",
                table: "Boats");

            migrationBuilder.DropIndex(
                name: "IX_Boats_BoatTypeId",
                table: "Boats");

            migrationBuilder.DropColumn(
                name: "BoatTypeId",
                table: "Boats");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PayTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BoatTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Boats_TypeId",
                table: "Boats",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boats_BoatTypes_TypeId",
                table: "Boats",
                column: "TypeId",
                principalTable: "BoatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boats_BoatTypes_TypeId",
                table: "Boats");

            migrationBuilder.DropIndex(
                name: "IX_Boats_TypeId",
                table: "Boats");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PayTypes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BoatTypes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "BoatTypeId",
                table: "Boats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boats_BoatTypeId",
                table: "Boats",
                column: "BoatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boats_BoatTypes_BoatTypeId",
                table: "Boats",
                column: "BoatTypeId",
                principalTable: "BoatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
