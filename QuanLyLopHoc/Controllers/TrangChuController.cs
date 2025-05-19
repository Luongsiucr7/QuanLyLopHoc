using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyLopHoc.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly AppDbContext context;

        public TrangChuController(AppDbContext context)
        {
            this.context = context;
        }


        [Authorize(Roles = "Admin")]      
        public IActionResult Index()
        {
            var lopHoc = context.LopHocs.Where(x => x.TrangThai == 1).ToList();
            return View(lopHoc);
        }
        [Authorize(Roles = "GiaoVien")]
        public IActionResult IndexGiaoVien()
        {
            var idGiaoVien = HttpContext.Session.GetInt32("Id");
            var lopHoc = context.LopGiaoViens
                   .Where(lm => lm.IdNguoiDung == idGiaoVien && lm.TrangThai == 1)
                   .Select(lm => lm.IdLopHoc)
                   .Distinct()
                   .Join(context.LopHocs,
                         idLop => idLop,
                         lh => lh.Id,
                         (idLop, lh) => lh)
                   .Where(lh => lh.TrangThai == 1)
                   .ToList();
            return View(lopHoc);
        }

        [Authorize(Roles = "HocSinh")]
        public IActionResult IndexHocSinh()
        {
            var lopHoc = context.LopHocs.Where(x => x.TrangThai == 1).ToList();
            return View(lopHoc);
        }

    }
}
