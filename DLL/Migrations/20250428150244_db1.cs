using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLL.Migrations
{
    /// <inheritdoc />
    public partial class db1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lop_hoc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_lop = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, computedColumnSql: "('LH'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", stored: true),
                    ten_lop = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    khoi_lop = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__lop_hoc__3213E83FE4ECF793", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mon_hoc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_mon = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, computedColumnSql: "('MH'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", stored: true),
                    ten_mon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mon_hoc__3213E83F3D8D09B4", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nguoi_dung",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nguoi_dung = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, computedColumnSql: "('ND'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", stored: true),
                    ten_nguoi_dung = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    dia_chi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    so_dien_thoai = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    nhan_xet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    gioi_tinh = table.Column<bool>(type: "bit", nullable: false),
                    ngay_sinh = table.Column<DateOnly>(type: "date", nullable: false),
                    mat_khau = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    vai_tro = table.Column<int>(type: "int", nullable: false),
                    ten_link_anh = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    id_lop_hoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__nguoi_du__3213E83FA9AD549F", x => x.id);
                    table.ForeignKey(
                        name: "FK__nguoi_dun__id_lo__4D94879B",
                        column: x => x.id_lop_hoc,
                        principalTable: "lop_hoc",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "diem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_diem = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false, computedColumnSql: "('DH'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", stored: true),
                    ten_diem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    so_diem = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    id_mon_hoc = table.Column<int>(type: "int", nullable: true),
                    id_nguoi_dung = table.Column<int>(type: "int", nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__diem__3213E83FAD09625C", x => x.id);
                    table.ForeignKey(
                        name: "FK__diem__id_mon_hoc__5070F446",
                        column: x => x.id_mon_hoc,
                        principalTable: "mon_hoc",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__diem__id_nguoi_d__5165187F",
                        column: x => x.id_nguoi_dung,
                        principalTable: "nguoi_dung",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "lop_giao_vien",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_nguoi_dung = table.Column<int>(type: "int", nullable: true),
                    id_lop_hoc = table.Column<int>(type: "int", nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__lop_giao__3213E83F3FDD8286", x => x.id);
                    table.ForeignKey(
                        name: "FK__lop_giao___id_lo__5535A963",
                        column: x => x.id_lop_hoc,
                        principalTable: "lop_hoc",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__lop_giao___id_ng__5441852A",
                        column: x => x.id_nguoi_dung,
                        principalTable: "nguoi_dung",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lop_mon",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_lop = table.Column<int>(type: "int", nullable: true),
                    id_mon_hoc = table.Column<int>(type: "int", nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    id_nguoi_dung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__lop_mon__3213E83F1B5F446A", x => x.id);
                    table.ForeignKey(
                        name: "FK__lop_mon__id_lop__5812160E",
                        column: x => x.id_lop,
                        principalTable: "lop_hoc",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__lop_mon__id_mon___59063A47",
                        column: x => x.id_mon_hoc,
                        principalTable: "mon_hoc",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__lop_mon__id_nguo__59FA5E80",
                        column: x => x.id_nguoi_dung,
                        principalTable: "nguoi_dung",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_diem_id_mon_hoc",
                table: "diem",
                column: "id_mon_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_diem_id_nguoi_dung",
                table: "diem",
                column: "id_nguoi_dung");

            migrationBuilder.CreateIndex(
                name: "IX_lop_giao_vien_id_lop_hoc",
                table: "lop_giao_vien",
                column: "id_lop_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_lop_giao_vien_id_nguoi_dung",
                table: "lop_giao_vien",
                column: "id_nguoi_dung");

            migrationBuilder.CreateIndex(
                name: "IX_lop_mon_id_lop",
                table: "lop_mon",
                column: "id_lop");

            migrationBuilder.CreateIndex(
                name: "IX_lop_mon_id_mon_hoc",
                table: "lop_mon",
                column: "id_mon_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_lop_mon_id_nguoi_dung",
                table: "lop_mon",
                column: "id_nguoi_dung");

            migrationBuilder.CreateIndex(
                name: "IX_nguoi_dung_id_lop_hoc",
                table: "nguoi_dung",
                column: "id_lop_hoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diem");

            migrationBuilder.DropTable(
                name: "lop_giao_vien");

            migrationBuilder.DropTable(
                name: "lop_mon");

            migrationBuilder.DropTable(
                name: "mon_hoc");

            migrationBuilder.DropTable(
                name: "nguoi_dung");

            migrationBuilder.DropTable(
                name: "lop_hoc");
        }
    }
}
