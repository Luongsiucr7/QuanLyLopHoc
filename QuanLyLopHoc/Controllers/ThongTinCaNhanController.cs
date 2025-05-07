using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class ThongTinCaNhanController : Controller
    {
        private readonly AppDbContext context;

        public ThongTinCaNhanController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
         var userId = HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); 
            }
            var nguoiDung = context.NguoiDungs.Find(userId);
            if (nguoiDung == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }
            return View(nguoiDung);
        }

        public IActionResult IndexHocSinh()
        {
            return View();
        }
    }
}
