using DLL.DTO;
using System;
using DLL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

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
        [HttpPost]
        public async Task<IActionResult> Login(NguoiDung nguoiDung)
        {
            var user = context.NguoiDungs
                .FirstOrDefault(x => x.Email == nguoiDung.Email && x.MatKhau == nguoiDung.MatKhau);

            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role,
                     user.VaiTro == 1 ? "GiaoVien" :
                     user.VaiTro == 2 ? "Admin" :
                     "HocSinh"),
            new Claim("VaiTro", user.VaiTro.ToString())
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("VaiTro", user.VaiTro);
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("TenNguoiDung", user.TenNguoiDung);

                if (user.VaiTro == 1)
                    return RedirectToAction("IndexGiaoVien", "TrangChu");
                else if (user.VaiTro == 0)
                    return RedirectToAction("IndexHocSinh", "TrangChu");
                else
                    return RedirectToAction("Index", "TrangChu");
            }

            ViewBag.Error = "Sai email hoặc mật khẩu.";
            return View();
        }  

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult DangKy()
        {
            ViewBag.DanhSachLop = new SelectList(context.LopHocs.ToList(), " Id", "TenLop");
            return View(new HocSinhDto());
        }

        [HttpPost]
        public IActionResult DangKy(HocSinhDto hocSinhDto)
        {
            if (!ModelState.IsValid)
            {
                return View(hocSinhDto);
            }       
                var hocSinh = new NguoiDung
            {
                TenNguoiDung = hocSinhDto.TenNguoiDung,
                Email = hocSinhDto.Email,
                SoDienThoai = hocSinhDto.SoDienThoai,
                DiaChi = hocSinhDto.DiaChi,
                GioiTinh = hocSinhDto.GioiTinh,
                NgaySinh = DateOnly.FromDateTime(hocSinhDto.NgaySinh),
                MatKhau = hocSinhDto.MatKhau,
                VaiTro = 0,       
                TrangThai = 1,
                IdLopHoc = hocSinhDto.IdLopHoc
            };
            context.NguoiDungs.Add(hocSinh);
            context.SaveChanges();
            return RedirectToAction("Login","Account");
        }
    }
}
