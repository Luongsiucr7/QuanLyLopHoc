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
            var lopHoc = context.LopHocs.ToList();
            return View(lopHoc);
        }
        [Authorize(Roles = "GiaoVien")]
        public IActionResult IndexGiaoVien()
        {
            var lopHoc = context.LopHocs.ToList();
            return View(lopHoc);
        }

        [Authorize(Roles = "HocSinh")]
        public IActionResult IndexHocSinh()
        {
            var lopHoc = context.LopHocs.ToList();
            return View(lopHoc);
        }

    }
}
