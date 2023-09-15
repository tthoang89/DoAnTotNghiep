using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class ThemDanhGia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_NguoiDung_IDNguoiDung",
                table: "GioHang");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NguoiDung_IDNguoiDung",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuTichDiem_NguoiDung_IDNguoiDung",
                table: "LichSuTichDiem");

            migrationBuilder.DropForeignKey(
                name: "FK_ThuocTinhSanPham_LoaiSP_IDLoaiSP",
                table: "ThuocTinhSanPham");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.RenameColumn(
                name: "IDLoaiSP",
                table: "ThuocTinhSanPham",
                newName: "IDSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_ThuocTinhSanPham_IDLoaiSP",
                table: "ThuocTinhSanPham",
                newName: "IX_ThuocTinhSanPham_IDSanPham");

            migrationBuilder.RenameColumn(
                name: "IDNguoiDung",
                table: "LichSuTichDiem",
                newName: "IDKhachHang");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuTichDiem_IDNguoiDung",
                table: "LichSuTichDiem",
                newName: "IX_LichSuTichDiem_IDKhachHang");

            migrationBuilder.RenameColumn(
                name: "IDNguoiDung",
                table: "HoaDon",
                newName: "IDNhanVien");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_IDNguoiDung",
                table: "HoaDon",
                newName: "IX_HoaDon_IDNhanVien");

            migrationBuilder.RenameColumn(
                name: "IDNguoiDung",
                table: "GioHang",
                newName: "IDKhachHang");

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    IDKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Password = table.Column<string>(type: "varchar(15)", nullable: false),
                    GioiTinh = table.Column<int>(type: "int", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SDT = table.Column<string>(type: "varchar(10)", nullable: false),
                    DiemTich = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDVaiTro = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.IDKhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHang_GioHang_IDKhachHang",
                        column: x => x.IDKhachHang,
                        principalTable: "GioHang",
                        principalColumn: "IDKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDVaiTro = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NhanVien_VaiTro_IDVaiTro",
                        column: x => x.IDVaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Sao = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDKhachHang = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DanhGia_BienThe_IDBienThe",
                        column: x => x.IDBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGia_KhachHang_IDKhachHang",
                        column: x => x.IDKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "IDKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_IDBienThe",
                table: "DanhGia",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_IDKhachHang",
                table: "DanhGia",
                column: "IDKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_IDVaiTro",
                table: "NhanVien",
                column: "IDVaiTro");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NhanVien_IDNhanVien",
                table: "HoaDon",
                column: "IDNhanVien",
                principalTable: "NhanVien",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuTichDiem_KhachHang_IDKhachHang",
                table: "LichSuTichDiem",
                column: "IDKhachHang",
                principalTable: "KhachHang",
                principalColumn: "IDKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_ThuocTinhSanPham_SanPham_IDSanPham",
                table: "ThuocTinhSanPham",
                column: "IDSanPham",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NhanVien_IDNhanVien",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuTichDiem_KhachHang_IDKhachHang",
                table: "LichSuTichDiem");

            migrationBuilder.DropForeignKey(
                name: "FK_ThuocTinhSanPham_SanPham_IDSanPham",
                table: "ThuocTinhSanPham");

            migrationBuilder.DropTable(
                name: "DanhGia");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.RenameColumn(
                name: "IDSanPham",
                table: "ThuocTinhSanPham",
                newName: "IDLoaiSP");

            migrationBuilder.RenameIndex(
                name: "IX_ThuocTinhSanPham_IDSanPham",
                table: "ThuocTinhSanPham",
                newName: "IX_ThuocTinhSanPham_IDLoaiSP");

            migrationBuilder.RenameColumn(
                name: "IDKhachHang",
                table: "LichSuTichDiem",
                newName: "IDNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuTichDiem_IDKhachHang",
                table: "LichSuTichDiem",
                newName: "IX_LichSuTichDiem_IDNguoiDung");

            migrationBuilder.RenameColumn(
                name: "IDNhanVien",
                table: "HoaDon",
                newName: "IDNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_IDNhanVien",
                table: "HoaDon",
                newName: "IX_HoaDon_IDNguoiDung");

            migrationBuilder.RenameColumn(
                name: "IDKhachHang",
                table: "GioHang",
                newName: "IDNguoiDung");

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDVaiTro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DiemTich = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    GioiTinh = table.Column<int>(type: "int", nullable: false),
                    Ho = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: false),
                    Password = table.Column<string>(type: "varchar(15)", nullable: false),
                    SDT = table.Column<string>(type: "varchar(10)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    TenDem = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.IDNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDung_VaiTro_IDVaiTro",
                        column: x => x.IDVaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_IDVaiTro",
                table: "NguoiDung",
                column: "IDVaiTro");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_NguoiDung_IDNguoiDung",
                table: "GioHang",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NguoiDung_IDNguoiDung",
                table: "HoaDon",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuTichDiem_NguoiDung_IDNguoiDung",
                table: "LichSuTichDiem",
                column: "IDNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "IDNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_ThuocTinhSanPham_LoaiSP_IDLoaiSP",
                table: "ThuocTinhSanPham",
                column: "IDLoaiSP",
                principalTable: "LoaiSP",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
