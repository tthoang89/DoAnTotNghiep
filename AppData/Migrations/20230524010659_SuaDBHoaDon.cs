using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaDBHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NguoiDung_IDNguoiDung",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_Voucher_IDVoucher",
                table: "HoaDon");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDVoucher",
                table: "HoaDon",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDNguoiDung",
                table: "HoaDon",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NguoiDung_IDNguoiDung",
                table: "HoaDon",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_Voucher_IDVoucher",
                table: "HoaDon",
                column: "IDVoucher",
                principalTable: "Voucher",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NguoiDung_IDNguoiDung",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_Voucher_IDVoucher",
                table: "HoaDon");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDVoucher",
                table: "HoaDon",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IDNguoiDung",
                table: "HoaDon",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NguoiDung_IDNguoiDung",
                table: "HoaDon",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_Voucher_IDVoucher",
                table: "HoaDon",
                column: "IDVoucher",
                principalTable: "Voucher",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
