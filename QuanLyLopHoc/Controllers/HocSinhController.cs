using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;


namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class HocSinhController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public HocSinhController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var danhSachHocSinh = context.NguoiDungs
                .Include(x => x.IdLopHocNavigation)
                .Where(x => x.VaiTro == 0)
                .ToList();

            var totalItemCount = danhSachHocSinh.Count();
            var totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            var pagedHocSinh = danhSachHocSinh
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.DanhSachHocSinh = pagedHocSinh;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.DanhSachLop = new SelectList(context.LopHocs.ToList(), "Id", "TenLop");

            return View(new HocSinhDto());
        }
        [HttpPost]
        public IActionResult Index(HocSinhDto hocSinhDto, IFormFile TenLinkAnh)
        {
            if (!ModelState.IsValid)
            {
                return View(hocSinhDto);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(TenLinkAnh!.FileName);

            string imageFullPath = environment.WebRootPath + "/hocsinh/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                TenLinkAnh.CopyTo(stream);
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
                TenLinkAnh = newFileName, // lưu tên file ảnh
                TrangThai = 1,
                IdLopHoc = hocSinhDto.IdLopHoc
            };
            context.NguoiDungs.Add(hocSinh);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var hocSinh = context.NguoiDungs.Find(id);
            if (hocSinh == null)
            {
                return RedirectToAction("Index", "HocSinh");
            }

            var hocSinhDto = new HocSinhDto
            {
                TenNguoiDung = hocSinh.TenNguoiDung,
                Email = hocSinh.Email,
                DiaChi = hocSinh.DiaChi,
                SoDienThoai = hocSinh.SoDienThoai,
                NgaySinh = hocSinh.NgaySinh.ToDateTime(TimeOnly.MinValue),
                MatKhau = hocSinh.MatKhau,
                TrangThai = hocSinh.TrangThai
            };
            ViewData["Id"] = hocSinh.Id;
            ViewData["LopId"] = hocSinh.IdLopHoc;
            ViewData["TenLinkAnh"] = hocSinh.TenLinkAnh;
            return View(hocSinhDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, HocSinhDto hocSinhDto, IFormFile? TenLinkAnh)
        {
            if (!ModelState.IsValid)
            {
                return View(hocSinhDto);
            }
            var hocSinh = context.NguoiDungs.Find(id);
            if (hocSinh == null)
            {
                return RedirectToAction("Index", "HocSinh");
            }
            string newFileName = hocSinh.TenLinkAnh;
            if(hocSinhDto.TenLinkAnh != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(TenLinkAnh.FileName);
                string imageFullPath = environment.WebRootPath + "/hocsinh/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    TenLinkAnh.CopyTo(stream);
                }
                string oldImageFullPath = environment.WebRootPath + "/hocsinh/" + hocSinh.TenLinkAnh;
                System.IO.File.Delete(oldImageFullPath);
            }
                
              // lưu tên file ảnh
            
            hocSinh.TenNguoiDung = hocSinhDto.TenNguoiDung;
            hocSinh.Email = hocSinhDto.Email;
            hocSinh.SoDienThoai = hocSinhDto.SoDienThoai;
            hocSinh.DiaChi = hocSinhDto.DiaChi;
            hocSinh.GioiTinh = hocSinhDto.GioiTinh;
            hocSinh.NgaySinh = DateOnly.FromDateTime(hocSinhDto.NgaySinh);
            hocSinh.MatKhau = hocSinhDto.MatKhau;
            hocSinh.TenLinkAnh = newFileName;
            hocSinh.TrangThai = hocSinhDto.TrangThai;
            hocSinh.IdLopHoc = hocSinhDto.IdLopHoc;

            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
