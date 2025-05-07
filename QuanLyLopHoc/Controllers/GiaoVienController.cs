using System.Linq;
using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class GiaoVienController : Controller
    {

        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public GiaoVienController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var giaoVienQuery = context.NguoiDungs
                .Where(x => x.VaiTro == 1)
                .ToList();

            var monTheoNguoiDung = context.LopMons
                .Join(context.MonHocs,
                    lm => lm.IdMonHoc,
                    mh => mh.Id,
                    (lm, mh) => new { lm.IdNguoiDung, mh.TenMon })
                .GroupBy(x => x.IdNguoiDung)
                .Select(g => new KeyValuePair<int, string>(
                    (int)g.Key,
                    string.Join(", ", g.Select(x => x.TenMon).Distinct().ToArray())  // Lọc các môn học trùng lặp và chuyển thành mảng
                ))
                .ToDictionary(x => x.Key, x => x.Value);

            var totalItemCount = giaoVienQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            var pagedGiaoVien = giaoVienQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.MonTheoNguoiDungId = monTheoNguoiDung;
            ViewBag.DanhSachGiaoVien = pagedGiaoVien;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(new GiaoVienDto());
        }

        [HttpPost]
        public IActionResult Index(GiaoVienDto giaoVienDto, IFormFile TenLinkAnh)
        {
            if (!ModelState.IsValid)
            {
                return View(giaoVienDto);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(TenLinkAnh!.FileName);
            string imageFullPath = environment.WebRootPath + "/giaovien/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                TenLinkAnh.CopyTo(stream);
            }
            var giaoVien = new NguoiDung
            {
                TenNguoiDung = giaoVienDto.TenNguoiDung,
                Email = giaoVienDto.Email,
                SoDienThoai = giaoVienDto.SoDienThoai,
                DiaChi = giaoVienDto.DiaChi,
                GioiTinh = giaoVienDto.GioiTinh,
                NgaySinh = DateOnly.FromDateTime(giaoVienDto.NgaySinh),
                MatKhau = giaoVienDto.MatKhau,
                VaiTro = 1,
                TenLinkAnh = newFileName,
                TrangThai = 1
            };
            context.NguoiDungs.Add(giaoVien);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var giaoVien = context.NguoiDungs.Find(id);
            if (giaoVien == null)
            {
                return RedirectToAction("Index","GiaoVien");
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

        [HttpPost]
        public IActionResult Edit(int id, GiaoVienDto giaoVienDto, IFormFile? TenLinkAnh)
        {
            if (!ModelState.IsValid)
            {
                return View(giaoVienDto);
            }
            var giaoVien = context.NguoiDungs.Find(id);
            if (giaoVien == null)
            {
                return RedirectToAction("Index", "GiaoVien");
            }
            if (TenLinkAnh != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(TenLinkAnh.FileName);
                string imageFullPath = environment.WebRootPath + "/giaovien/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    TenLinkAnh.CopyTo(stream);
                }
                giaoVien.TenLinkAnh = newFileName;
            }
            giaoVien.TenNguoiDung = giaoVienDto.TenNguoiDung;
            giaoVien.Email = giaoVienDto.Email;
            giaoVien.SoDienThoai = giaoVienDto.SoDienThoai;
            giaoVien.DiaChi = giaoVienDto.DiaChi;
            giaoVien.GioiTinh = giaoVienDto.GioiTinh;
            giaoVien.NgaySinh = DateOnly.FromDateTime(giaoVienDto.NgaySinh);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
