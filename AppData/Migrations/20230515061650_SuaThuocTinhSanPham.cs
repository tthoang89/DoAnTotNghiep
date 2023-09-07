using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaThuocTinhSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietBienThe_ThuocTinhSanPham_IDThuocTinhSanPham",
                table: "ChiTietBienThe");

            migrationBuilder.DropForeignKey(
                name: "FK_ThuocTinhSanPham_SanPham_IDSanPham",
                table: "ThuocTinhSanPham");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietBienThe_IDThuocTinhSanPham",
                table: "ChiTietBienThe");

            migrationBuilder.DropColumn(
                name: "IDThuocTinhSanPham",
                table: "ChiTietBienThe");

            migrationBuilder.RenameColumn(
                name: "IDSanPham",
                table: "ThuocTinhSanPham",
                newName: "IDLoaiSP");

            migrationBuilder.RenameIndex(
                name: "IX_ThuocTinhSanPham_IDSanPham",
                table: "ThuocTinhSanPham",
                newName: "IX_ThuocTinhSanPham_IDLoaiSP");

            migrationBuilder.AddForeignKey(
                name: "FK_ThuocTinhSanPham_LoaiSP_IDLoaiSP",
                table: "ThuocTinhSanPham",
                column: "IDLoaiSP",
                principalTable: "LoaiSP",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThuocTinhSanPham_LoaiSP_IDLoaiSP",
                table: "ThuocTinhSanPham");

            migrationBuilder.RenameColumn(
                name: "IDLoaiSP",
                table: "ThuocTinhSanPham",
                newName: "IDSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_ThuocTinhSanPham_IDLoaiSP",
                table: "ThuocTinhSanPham",
                newName: "IX_ThuocTinhSanPham_IDSanPham");

            migrationBuilder.AddColumn<Guid>(
                name: "IDThuocTinhSanPham",
                table: "ChiTietBienThe",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietBienThe_IDThuocTinhSanPham",
                table: "ChiTietBienThe",
                column: "IDThuocTinhSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietBienThe_ThuocTinhSanPham_IDThuocTinhSanPham",
                table: "ChiTietBienThe",
                column: "IDThuocTinhSanPham",
                principalTable: "ThuocTinhSanPham",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ThuocTinhSanPham_SanPham_IDSanPham",
                table: "ThuocTinhSanPham",
                column: "IDSanPham",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
