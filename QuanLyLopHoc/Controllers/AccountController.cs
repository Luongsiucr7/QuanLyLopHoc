using DLL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext context;

        public AccountController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    var user = context.NguoiDungs.FirstOrDefault(u => u.Email == username && u.MatKhau == password);
        //    if (user == null)
        //    {
        //        ViewData["ErrorLogin"] = "Không tìm thấy tài khoản này";
        //    }
        //    else
        //    {
        //        // Set authentication cookie
        //        HttpContext.Response.Cookies.Append("username", username);
        //        return RedirectToAction("Index", "TrangChu");
        //    }
            
        //    return View();
        //}

        public IActionResult DangKy()
        {
            ViewBag.DanhSachLop = context.LopHocs.ToList();
            return View(new NguoiDung());
        }
    }
}
