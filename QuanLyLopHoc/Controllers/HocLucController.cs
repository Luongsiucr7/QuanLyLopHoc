using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    [Authorize(Roles ="HocSinh")]
    public class HocLucController : Controller
    {
        private readonly AppDbContext _context;

        public HocLucController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var idHocSinh = HttpContext.Session.GetInt32("Id");
            var idLop = HttpContext.Session.GetInt32("IdLop");

            var query = _context.NguoiDungs
                .Where(nd => nd.VaiTro == 0 && nd.TrangThai == 1 && nd.Id == idHocSinh)
                .Join(_context.LopHocs.Where(lh => lh.TrangThai == 1 && lh.Id == idLop),
                    nd => nd.IdLopHoc,
                    lh => lh.Id,
                    (nd, lh) => new { NguoiDung = nd, LopHoc = lh })
                .Join(_context.Diems.Where(d => d.TrangThai == 1),
                    x => x.NguoiDung.Id,
                    d => d.IdNguoiDung,
                    (x, d) => new { x.NguoiDung, x.LopHoc, Diem = d })
                .Join(_context.MonHocs.Where(mh => mh.TrangThai == 1),
                    x => x.Diem.IdMonHoc,
                    mh => mh.Id,
                    (x, mh) => new
                    {
                        x.NguoiDung,
                        x.LopHoc,
                        x.Diem,
                        MonHoc = mh.TenMon,
                        TenDiem = x.Diem.TenDiem,
                        SoDiem = x.Diem.SoDiem
                    })
                .GroupBy(x => new { x.NguoiDung.Id, x.NguoiDung.TenNguoiDung, x.LopHoc.TenLop })
                .Select(g => new HocLucDto
                {
                    TenHocSinh = g.Key.TenNguoiDung,
                    LopHoc = g.Key.TenLop,

                    // Điểm giữa kỳ
                    DiemToan_GK = g.Where(x => x.MonHoc == "Toán" && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemVan_GK = g.Where(x => x.MonHoc == "Văn" && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemLichSu_GK = g.Where(x => x.MonHoc == "Lịch sử" && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemNgoaiNgu_GK = g.Where(x => x.MonHoc == "Ngoại ngữ" && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemKhoaHoc_GK = g.Where(x => x.MonHoc == "Khoa học" && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemTheDuc_GK = g.Where(x => x.MonHoc == "Thể dục" && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemTinHoc_GK = g.Where(x => x.MonHoc == "Tin học" && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),

                    // Điểm cuối kỳ
                    DiemToan_CK = g.Where(x => x.MonHoc == "Toán" && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemVan_CK = g.Where(x => x.MonHoc == "Văn" && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemLichSu_CK = g.Where(x => x.MonHoc == "Lịch sử" && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemNgoaiNgu_CK = g.Where(x => x.MonHoc == "Ngoại ngữ" && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemKhoaHoc_CK = g.Where(x => x.MonHoc == "Khoa học" && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemTheDuc_CK = g.Where(x => x.MonHoc == "Thể dục" && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                    DiemTinHoc_CK = g.Where(x => x.MonHoc == "Tin học" && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
              
                })
                .FirstOrDefault();

            return View(query);
        }
    }
}
