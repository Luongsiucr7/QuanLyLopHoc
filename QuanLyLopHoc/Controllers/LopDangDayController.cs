using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    public class LopDangDayController : Controller
    {
        private readonly AppDbContext context;

        public LopDangDayController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int? idLopHoc)
        {
            var idGiaovien = HttpContext.Session.GetInt32("Id");
            if (idGiaovien == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách lớp mà giáo viên đang dạy
            var lopDangDayList = (from lm in context.LopMons
                                  join lh in context.LopHocs on lm.IdLop equals lh.Id
                                  where lm.IdNguoiDung == idGiaovien && lm.TrangThai == 1
                                  select new SelectListItem
                                  {
                                      Value = lh.Id.ToString(),
                                      Text = lh.TenLop
                                  }).Distinct().ToList();

            ViewBag.LopDangDay = lopDangDayList;

            // Nếu không chọn lớp, không thực hiện truy vấn bảng điểm
            if (idLopHoc == null)
            {
                return View();
            }


            var data = (from lm in context.LopMons
                        join mh in context.MonHocs on lm.IdMonHoc equals mh.Id
                        join lh in context.LopHocs on lm.IdLop equals lh.Id
                        join nd in context.NguoiDungs on lh.Id equals nd.IdLopHoc
                        where lm.IdLop == idLopHoc
                           && lm.IdNguoiDung == idGiaovien
                           && lm.TrangThai == 1
                           && nd.VaiTro == 0
                           && nd.TrangThai == 1
                        join d in context.Diems
                            on new { ndId = nd.Id, mhId = mh.Id }
                            equals new { ndId = d.IdNguoiDung.Value, mhId = d.IdMonHoc.Value } into diemJoin
                        from d in diemJoin.DefaultIfEmpty()
                        group d by new { nd.Id, nd.TenNguoiDung, mh.TenMon } into g
                        select new LopDangDayDto
                        {
                            TenHocSinh = g.Key.TenNguoiDung,
                            TenMon = g.Key.TenMon,
                            DiemGiuaKy = g.Where(x => x != null && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                            DiemCuoiKy = g.Where(x => x != null && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault()
                        }).ToList();

            return View(data);
        }

        [HttpPost]
        public IActionResult Index(int? idLopHoc, string? tenMon)
        {



         return RedirectToAction("Index");
        }
    }
}
