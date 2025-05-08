using System;
using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class ThongTinCaNhanController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public ThongTinCaNhanController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var giaoVien = context.NguoiDungs.Find(userId);
            if (giaoVien == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }
            var giaoVienDto = new GiaoVienDto
            {
                TenNguoiDung = giaoVien.TenNguoiDung,
                Email = giaoVien.Email,
                DiaChi = giaoVien.DiaChi,
                SoDienThoai = giaoVien.SoDienThoai,
                GioiTinh = giaoVien.GioiTinh,
                NgaySinh = giaoVien.NgaySinh.ToDateTime(TimeOnly.MinValue),
                TrangThai = giaoVien.TrangThai
            };

            ViewData["Id"] = giaoVien.Id;
            ViewData["TenLinkAnh"] = giaoVien.TenLinkAnh;
            return View(giaoVienDto);
        }

        public IActionResult IndexHocSinh()
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
            var hocSinhDto = new HocSinhDto
            {
                TenNguoiDung = nguoiDung.TenNguoiDung,
                Email = nguoiDung.Email,
                DiaChi = nguoiDung.DiaChi,
                SoDienThoai = nguoiDung.SoDienThoai,
                GioiTinh = nguoiDung.GioiTinh,
                NgaySinh = nguoiDung.NgaySinh.ToDateTime(TimeOnly.MinValue),
                TrangThai = nguoiDung.TrangThai
            };

            ViewData["Id"] = nguoiDung.Id;
            ViewData["TenLinkAnh"] = nguoiDung.TenLinkAnh;

            return View(hocSinhDto);
        }

        [HttpPost]
        public IActionResult Index(GiaoVienDto giaoVien, IFormFile TenLinkAnh)
        {
            var userId = HttpContext.Session.GetInt32("Id");
            var gv = context.NguoiDungs.FirstOrDefault(x => x.Id == userId);
            if (gv == null) return NotFound();

            gv.TenNguoiDung = giaoVien.TenNguoiDung;
            gv.Email = giaoVien.Email;
            gv.SoDienThoai = giaoVien.SoDienThoai;
            gv.DiaChi = giaoVien.DiaChi;
            gv.GioiTinh = giaoVien.GioiTinh;
            gv.NgaySinh = DateOnly.FromDateTime(giaoVien.NgaySinh);

            if (TenLinkAnh != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(TenLinkAnh.FileName);
                string path = Path.Combine(environment.WebRootPath, "giaovien", newFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    TenLinkAnh.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(gv.TenLinkAnh))
                {
                    string oldPath = Path.Combine(environment.WebRootPath, "giaovien", gv.TenLinkAnh);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                gv.TenLinkAnh = newFileName;
            }

            context.SaveChanges();

            ViewData["TenLinkAnh"] = gv.TenLinkAnh;
            ViewData["Id"] = gv.Id;

            return View(giaoVien); 
        }

        [HttpPost]
        public IActionResult IndexHocSinh(HocSinhDto hocSinhDto, IFormFile? TenLinkAnh)
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
           
            var hocSinh = context.NguoiDungs.Find(userId);
            if (hocSinh == null)
            {
                return RedirectToAction("Index");
            }
            hocSinh.TenNguoiDung = hocSinhDto.TenNguoiDung;
            hocSinh.Email = hocSinhDto.Email;
            hocSinh.SoDienThoai = hocSinhDto.SoDienThoai;
            hocSinh.DiaChi = hocSinhDto.DiaChi;
            hocSinh.GioiTinh = hocSinhDto.GioiTinh;
            hocSinh.NgaySinh = DateOnly.FromDateTime(hocSinhDto.NgaySinh);

            if (TenLinkAnh != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(TenLinkAnh.FileName);
                string path = Path.Combine(environment.WebRootPath, "hocsinh", newFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    TenLinkAnh.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(hocSinh.TenLinkAnh))
                {
                    string oldPath = Path.Combine(environment.WebRootPath, "hocsinh", hocSinh.TenLinkAnh);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                hocSinh.TenLinkAnh = newFileName;
            }

            context.SaveChanges();


            ViewData["TenLinkAnh"] = hocSinh.TenLinkAnh;
            ViewData["Id"] = hocSinh.Id;

            return View(hocSinhDto);
        }
    }
}
