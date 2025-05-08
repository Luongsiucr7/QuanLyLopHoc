using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    [Authorize(Roles ="HocSinh")]
    public class LopDangHocController : Controller
    {
        private readonly AppDbContext _context;

        public LopDangHocController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DiemGiuaKy()
        {
            var idLop = HttpContext.Session.GetInt32("IdLop");
            var query = _context.NguoiDungs
                .Where(nd => nd.VaiTro == 0 && nd.TrangThai == 1) // Học sinh
            .Join(_context.LopHocs.Where(lh => lh.TrangThai == 1 && lh.Id == idLop), // Lớp học
            nd => nd.IdLopHoc,
                    lh => lh.Id,
            (nd, lh) => new { NguoiDung = nd, LopHoc = lh })
                .Join(_context.Diems.Where(d => d.TrangThai == 1 && d.TenDiem == "Giữa Kỳ"), // Điểm
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

            return View(query);
        }
        public IActionResult DiemCuoiKy()
        {
            var idLop = HttpContext.Session.GetInt32("IdLop");
            var query = _context.NguoiDungs
                .Where(nd => nd.VaiTro == 0 && nd.TrangThai == 1) // Học sinh
            .Join(_context.LopHocs.Where(lh => lh.TrangThai == 1 && lh.Id == idLop), // Lớp học
            nd => nd.IdLopHoc,
                    lh => lh.Id,
            (nd, lh) => new { NguoiDung = nd, LopHoc = lh })
                .Join(_context.Diems.Where(d => d.TrangThai == 1 && d.TenDiem == "Cuối Kỳ"), // Điểm
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

            return View(query);
        }
    }
}
