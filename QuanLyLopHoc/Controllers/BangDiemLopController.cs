using System.Data;
//using CrystalDecisions.CrystalReports.Engine;
using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;

namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class BangDiemLopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString = @"Data Source=LUONGNDDPH50650\SQLEXPRESS;Initial Catalog=QuanLyLopHoc3;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;MultipleActiveResultSets=True";
        private readonly IWebHostEnvironment _env;

        public BangDiemLopController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private static double? TinhDiem(Diem? diemGK, Diem? diemCK)
        {
            if (diemGK != null && diemCK != null)
            {
                return (double?)Math.Round((diemGK.SoDiem * 1 + diemCK.SoDiem * 2) / 3, 2); // làm tròn 2 chữ số
            }

            return null;
        }

        private double? DiemTongKet(int idNguoiDung)
        {
            var monHocList = new List<string>
            {
                "Toán", "Văn", "Lịch sử", "Ngoại ngữ", "Khoa học", "Thể dục", "Tin học"
            };

            var diemList = monHocList
                .Select(mon => TinhDiemTheoMon(idNguoiDung, mon))
                .Where(d => d.HasValue)
                .Select(d => d.Value)
                .ToList();

            if (diemList.Count == 0)
                return null;

            var tongDiem = diemList.Sum();
            var diemTB = tongDiem / diemList.Count;

            return Math.Round(diemTB, 2);
        }
        private double? TinhDiemTheoMon(int idNguoiDung, string tenMon)
        {
            var diemGK = _context.Diems
                .Include(d => d.IdMonHocNavigation)
                .FirstOrDefault(d =>
                    d.IdNguoiDung == idNguoiDung &&
                    d.TenDiem == "Giữa Kỳ" &&
                    d.TrangThai == 1 &&
                    d.IdMonHocNavigation.TenMon == tenMon);

            var diemCK = _context.Diems
                .Include(d => d.IdMonHocNavigation)
                .FirstOrDefault(d =>
                    d.IdNguoiDung == idNguoiDung &&
                    d.TenDiem == "Cuối Kỳ" &&
                    d.TrangThai == 1 &&
                    d.IdMonHocNavigation.TenMon == tenMon);

            return TinhDiem(diemGK, diemCK);
        }

        public IActionResult Index(int idLop, string tenDiem)
        {
            List<BangDiemHocSinhDto> query;

            if (tenDiem.Equals("Tổng Kết"))
            {
                query = _context.NguoiDungs
                   .Where(nd => nd.VaiTro == 0 && nd.TrangThai == 1 && nd.IdLopHoc == idLop)
                   .Join(_context.LopHocs.Where(lh => lh.TrangThai == 1),
                       nd => nd.IdLopHoc,
                       lh => lh.Id,
                       (nd, lh) => new { NguoiDung = nd, LopHoc = lh })
                   .Join(_context.LopGiaoViens.Where(lgv => lgv.TrangThai == 1),
                       x => x.LopHoc.Id,
                       lgv => lgv.IdLopHoc,
                       (x, lgv) => new { x.NguoiDung, x.LopHoc, LopGiaoVien = lgv })
                   .Join(_context.NguoiDungs.Where(gv => gv.TrangThai == 1),
                       x => x.LopGiaoVien.IdNguoiDung,
                       gv => gv.Id,
                       (x, gv) => new { x.NguoiDung, x.LopHoc, GiaoVienChuNhiem = gv.TenNguoiDung })
                   .AsEnumerable()
                   .Select(hs => new BangDiemHocSinhDto
                   {
                       TenHocSinh = hs.NguoiDung.TenNguoiDung,
                       LopHoc = hs.LopHoc.TenLop,
                       GiaoVienChuNhiem = hs.GiaoVienChuNhiem,
                       DiemToan = (decimal?)TinhDiemTheoMon(hs.NguoiDung.Id, "Toán"),
                       DiemVan = (decimal?)TinhDiemTheoMon(hs.NguoiDung.Id, "Văn"),
                       DiemLichSu = (decimal?)TinhDiemTheoMon(hs.NguoiDung.Id, "Lịch sử"),
                       DiemNgoaiNgu = (decimal?)TinhDiemTheoMon(hs.NguoiDung.Id, "Ngoại ngữ"),
                       DiemKhoaHoc = (decimal?)TinhDiemTheoMon(hs.NguoiDung.Id, "Khoa học"),
                       DiemTheDuc = (decimal?)TinhDiemTheoMon(hs.NguoiDung.Id, "Thể dục"),
                       DiemTinHoc = (decimal?)TinhDiemTheoMon(hs.NguoiDung.Id, "Tin học"),
                       DiemTongKet = DiemTongKet(hs.NguoiDung.Id)

                   })
                   .OrderBy(x => x.LopHoc)
                   .ThenBy(x => x.TenHocSinh)
                   .ToList();
            }
            else
            {
                 query = _context.NguoiDungs
                      .Where(nd => nd.VaiTro == 0 && nd.TrangThai == 1) // Học sinh
                  .Join(_context.LopHocs.Where(lh => lh.TrangThai == 1 && lh.Id == idLop), // Lớp học
                  nd => nd.IdLopHoc,
                          lh => lh.Id,
                          (nd, lh) => new { NguoiDung = nd, LopHoc = lh })
                      .Join(_context.Diems.Where(d => d.TrangThai == 1 && d.TenDiem == tenDiem), // Điểm
                          x => x.NguoiDung.Id,
                          d => d.IdNguoiDung,
                          (x, d) => new { x.NguoiDung, x.LopHoc, Diem = d })
                      .Join(_context.MonHocs.Where(mh => mh.TrangThai == 1), // Môn học
                          x => x.Diem.IdMonHoc,
                          mh => mh.Id,
                          (x, mh) => new { x.NguoiDung, x.LopHoc, x.Diem, MonHoc = mh })
                      .GroupJoin(_context.LopGiaoViens.Where(lgv => lgv.TrangThai == 1), // Giáo viên chủ nhiệm
                          x => x.LopHoc.Id,
                          lgv => lgv.IdLopHoc,
                          (x, lgv) => new { x.NguoiDung, x.LopHoc, x.Diem, x.MonHoc, LopGiaoViens = lgv })
                      .SelectMany(x => x.LopGiaoViens.DefaultIfEmpty(),
                          (x, lgv) => new { x.NguoiDung, x.LopHoc, x.Diem, x.MonHoc, LopGiaoVien = lgv })
                      .GroupJoin(_context.NguoiDungs,
                          x => x.LopGiaoVien != null ? x.LopGiaoVien.IdNguoiDung : null,
                          gvcn => gvcn.Id,
                          (x, gvcn) => new { x.NguoiDung, x.LopHoc, x.Diem, x.MonHoc, x.LopGiaoVien, GiaoViens = gvcn })
                      .SelectMany(x => x.GiaoViens.DefaultIfEmpty(),
                          (x, gvcn) => new
                          {
                              IdHocSinh = x.NguoiDung.Id,
                              TenHocSinh = x.NguoiDung.TenNguoiDung,
                              LopHoc = x.LopHoc.TenLop,
                              GiaoVienChuNhiem = gvcn != null ? gvcn.TenNguoiDung : null,
                              MonHoc = x.MonHoc.TenMon,
                              DiemSo = x.Diem.SoDiem
                          })
                      .GroupBy(x => new { x.IdHocSinh, x.TenHocSinh, x.LopHoc, x.GiaoVienChuNhiem })
                      .Select(g => new BangDiemHocSinhDto
                      {
                          TenHocSinh = g.Key.TenHocSinh,
                          LopHoc = g.Key.LopHoc,
                          GiaoVienChuNhiem = g.Key.GiaoVienChuNhiem,
                          DiemToan = g.Where(x => x.MonHoc == "Toán").Select(x => x.DiemSo).FirstOrDefault(),
                          DiemVan = g.Where(x => x.MonHoc == "Văn").Select(x => x.DiemSo).FirstOrDefault(),
                          DiemLichSu = g.Where(x => x.MonHoc == "Lịch sử").Select(x => x.DiemSo).FirstOrDefault(),
                          DiemNgoaiNgu = g.Where(x => x.MonHoc == "Ngoại ngữ").Select(x => x.DiemSo).FirstOrDefault(),
                          DiemKhoaHoc = g.Where(x => x.MonHoc == "Khoa học").Select(x => x.DiemSo).FirstOrDefault(),
                          DiemTheDuc = g.Where(x => x.MonHoc == "Thể dục").Select(x => x.DiemSo).FirstOrDefault(),
                          DiemTinHoc = g.Where(x => x.MonHoc == "Tin học").Select(x => x.DiemSo).FirstOrDefault()
                        })
                      .OrderBy(x => x.LopHoc)
                      .ThenBy(x => x.TenHocSinh)
                      .ToList();
            }
            ViewBag.IdLop = idLop;
            ViewBag.TenDiem = tenDiem;
            return View(query);
        }

    
        public IActionResult XuatBaoCao(int idLop)
        {
            // Load RDLC
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "ReportMau.rdlc");
            LocalReport report = new LocalReport();
            report.ReportPath = path;

            var dtLop = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("BaoCaoTongKetLopTheoId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdLop", idLop);
                    conn.Open();
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dtLop);
                    }
                }
            }

            var dtHocSinh = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("BaoCaoChiTietHocSinhTheoLop1", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdLop", idLop);
                    conn.Open();
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dtHocSinh);
                    }
                }
            }

            report.DataSources.Add(new ReportDataSource("DataSet1", dtLop));
            report.DataSources.Add(new ReportDataSource("DataSet2", dtHocSinh));

            // Xuất PDF
            var pdfBytes = report.Render("PDF");

            return File(pdfBytes, "application/pdf", $"BangDiem_Lop_{idLop}.pdf");
        }

    }
}
