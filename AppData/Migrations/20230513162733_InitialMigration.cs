using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    GiaTri = table.Column<int>(type: "int", nullable: false),
                    NgayApDung = table.Column<DateTime>(type: "datetime", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSP",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDLoaiSPCha = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSP", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LoaiSP_LoaiSP_IDLoaiSPCha",
                        column: x => x.IDLoaiSPCha,
                        principalTable: "LoaiSP",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "QuyDoiDiem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoDiem = table.Column<int>(type: "int", nullable: false),
                    TiLeTichDiem = table.Column<int>(type: "int", nullable: false),
                    TiLeTieuDiem = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyDoiDiem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ThuocTinh",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "Datetime", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuocTinh", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    HinhThucGiamGia = table.Column<int>(type: "int", nullable: false),
                    SoTienCan = table.Column<int>(type: "int", nullable: false),
                    GiaTri = table.Column<int>(type: "int", nullable: false),
                    NgayApDung = table.Column<DateTime>(type: "datetime", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDLoaiSP = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SanPham_LoaiSP_IDLoaiSP",
                        column: x => x.IDLoaiSP,
                        principalTable: "LoaiSP",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiaTri",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IdThuocTinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaTri", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GiaTri_ThuocTinh_IdThuocTinh",
                        column: x => x.IdThuocTinh,
                        principalTable: "ThuocTinh",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    TenDem = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Ho = table.Column<string>(type: "nvarchar(15)", nullable: false),
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
                    table.PrimaryKey("PK_NguoiDung", x => x.IDNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDung_VaiTro_IDVaiTro",
                        column: x => x.IDVaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BienThe",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiaBan = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "Datetime", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    Anh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienThe", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BienThe_SanPham_IDSanPham",
                        column: x => x.IDSanPham,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuocTinhSanPham",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDThuocTinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuocTinhSanPham", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ThuocTinhSanPham_SanPham_IDSanPham",
                        column: x => x.IDSanPham,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThuocTinhSanPham_ThuocTinh_IDThuocTinh",
                        column: x => x.IDThuocTinh,
                        principalTable: "ThuocTinh",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.IDNguoiDung);
                    table.ForeignKey(
                        name: "FK_GioHang_NguoiDung_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime", nullable: true),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TienShip = table.Column<int>(type: "int", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TrangThaiGiaoHang = table.Column<int>(type: "int", nullable: false),
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDVoucher = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HoaDon_NguoiDung_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDon_Voucher_IDVoucher",
                        column: x => x.IDVoucher,
                        principalTable: "Voucher",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKhuyenMai",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKhuyenMai", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietKhuyenMai_BienThe_IDBienThe",
                        column: x => x.IDBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietKhuyenMai_KhuyenMai_IDKhuyenMai",
                        column: x => x.IDKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietBienThe",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDGiaTri = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDThuocTinhSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietBienThe", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietBienThe_BienThe_IDBienThe",
                        column: x => x.IDBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietBienThe_GiaTri_IDGiaTri",
                        column: x => x.IDGiaTri,
                        principalTable: "GiaTri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietBienThe_ThuocTinhSanPham_IDThuocTinhSanPham",
                        column: x => x.IDThuocTinhSanPham,
                        principalTable: "ThuocTinhSanPham",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHang",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    IDBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHang", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_BienThe_IDBienThe",
                        column: x => x.IDBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_GioHang_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "GioHang",
                        principalColumn: "IDNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonGia = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_BienThe_IDBienThe",
                        column: x => x.IDBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_HoaDon_IDHoaDon",
                        column: x => x.IDHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichSuTichDiem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Diem = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IDQuyDoiDiem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuTichDiem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LichSuTichDiem_HoaDon_IDHoaDon",
                        column: x => x.IDHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichSuTichDiem_NguoiDung_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "IDNguoiDung");
                    table.ForeignKey(
                        name: "FK_LichSuTichDiem_QuyDoiDiem_IDQuyDoiDiem",
                        column: x => x.IDQuyDoiDiem,
                        principalTable: "QuyDoiDiem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienThe_IDSanPham",
                table: "BienThe",
                column: "IDSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietBienThe_IDBienThe",
                table: "ChiTietBienThe",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietBienThe_IDGiaTri",
                table: "ChiTietBienThe",
                column: "IDGiaTri");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietBienThe_IDThuocTinhSanPham",
                table: "ChiTietBienThe",
                column: "IDThuocTinhSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_IDBienThe",
                table: "ChiTietGioHang",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_IDNguoiDung",
                table: "ChiTietGioHang",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_IDBienThe",
                table: "ChiTietHoaDon",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_IDHoaDon",
                table: "ChiTietHoaDon",
                column: "IDHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKhuyenMai_IDBienThe",
                table: "ChiTietKhuyenMai",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKhuyenMai_IDKhuyenMai",
                table: "ChiTietKhuyenMai",
                column: "IDKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_GiaTri_IdThuocTinh",
                table: "GiaTri",
                column: "IdThuocTinh");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDNguoiDung",
                table: "HoaDon",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDVoucher",
                table: "HoaDon",
                column: "IDVoucher");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuTichDiem_IDHoaDon",
                table: "LichSuTichDiem",
                column: "IDHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuTichDiem_IDNguoiDung",
                table: "LichSuTichDiem",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuTichDiem_IDQuyDoiDiem",
                table: "LichSuTichDiem",
                column: "IDQuyDoiDiem");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiSP_IDLoaiSPCha",
                table: "LoaiSP",
                column: "IDLoaiSPCha");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_IDVaiTro",
                table: "NguoiDung",
                column: "IDVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_IDLoaiSP",
                table: "SanPham",
                column: "IDLoaiSP");

            migrationBuilder.CreateIndex(
                name: "IX_ThuocTinhSanPham_IDSanPham",
                table: "ThuocTinhSanPham",
                column: "IDSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ThuocTinhSanPham_IDThuocTinh",
                table: "ThuocTinhSanPham",
                column: "IDThuocTinh");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietBienThe");

            migrationBuilder.DropTable(
                name: "ChiTietGioHang");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");

            migrationBuilder.DropTable(
                name: "ChiTietKhuyenMai");

            migrationBuilder.DropTable(
                name: "LichSuTichDiem");

            migrationBuilder.DropTable(
                name: "GiaTri");

            migrationBuilder.DropTable(
                name: "ThuocTinhSanPham");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "BienThe");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "QuyDoiDiem");

            migrationBuilder.DropTable(
                name: "ThuocTinh");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "LoaiSP");

            migrationBuilder.DropTable(
                name: "VaiTro");
        }
    }
}
