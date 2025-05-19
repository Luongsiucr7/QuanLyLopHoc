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

        private void LoadData(int page = 1, int pageSize = 10)
        {
            var danhSachHocSinh = context.NguoiDungs
                .Include(x => x.IdLopHocNavigation)
                .Where(x => x.VaiTro == 0 && x.TrangThai == 1)
                .ToList();
         
            var totalItemCount = context.NguoiDungs
                .Where(x => x.VaiTro == 0 && x.TrangThai == 1)
                .Count();
         
            var totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            var pagedHocSinh = context.NguoiDungs
                .Include(x => x.IdLopHocNavigation)
                .Where(x => x.VaiTro == 0 && x.TrangThai == 1)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.DanhSachHocSinh = pagedHocSinh;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.DanhSachLop = new SelectList(context.LopHocs.ToList(), "Id", "TenLop");
        }

        private void LoadDataGiaoVien(int page = 1, int pageSize = 10)
        {
            var idGiaoVien = HttpContext.Session.GetInt32("Id");

            var idLopHocChuNhiem = context.LopGiaoViens
                .Where(x => x.IdNguoiDung == idGiaoVien)
                .Select(x => x.IdLopHoc)
                .FirstOrDefault();

            var danhSachHocSinh = context.NguoiDungs
              .Include(x => x.IdLopHocNavigation)
              .Where(x => x.VaiTro == 0 && x.TrangThai == 1 && x.IdLopHoc == idLopHocChuNhiem)
              .ToList();

            var totalItemCount = context.NguoiDungs
               .Where(x => x.VaiTro == 0 && x.TrangThai == 1 && x.IdLopHoc == idLopHocChuNhiem)
               .Count();

            var totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            var pagedHocSinh = context.NguoiDungs
                .Include(x => x.IdLopHocNavigation)
                .Where(x => x.VaiTro == 0 && x.TrangThai == 1 && x.IdLopHoc == idLopHocChuNhiem)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            Console.WriteLine($"idLopHocChuNhiem = {idLopHocChuNhiem}");

            ViewBag.DanhSachHocSinh = pagedHocSinh;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.DanhSachLop = new SelectList(context.LopHocs.ToList(), "Id", "TenLop");
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var vaiTro = HttpContext.Session.GetInt32("VaiTro");

            if (vaiTro == 1) // 1 là giáo viên
            {
                LoadDataGiaoVien(page, pageSize);
            }
            else if (vaiTro == 2) // 2 là admin
            {
                LoadData(page, pageSize);
            }
            else
            {
                return RedirectToAction("Login", "Account"); // hoặc trả về 403
            }

            return View(new HocSinhDto());
        }


        [HttpPost]
        public IActionResult Index(HocSinhDto hocSinhDto, IFormFile TenLinkAnh, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                LoadData(page, pageSize);
                ViewBag.ShowModal = true;
                return View(hocSinhDto);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(TenLinkAnh.FileName);
            string path = Path.Combine(environment.WebRootPath, "hocsinh", newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
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
            hocSinh.TenNguoiDung = hocSinhDto.TenNguoiDung;
            hocSinh.Email = hocSinhDto.Email;
            hocSinh.SoDienThoai = hocSinhDto.SoDienThoai;
            hocSinh.DiaChi = hocSinhDto.DiaChi;
            hocSinh.GioiTinh = hocSinhDto.GioiTinh;
            hocSinh.NgaySinh = DateOnly.FromDateTime(hocSinhDto.NgaySinh);
            hocSinh.MatKhau = hocSinhDto.MatKhau;         
            hocSinh.TrangThai = hocSinhDto.TrangThai;
            hocSinh.IdLopHoc = hocSinhDto.IdLopHoc;

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
                    string oldPath = Path.Combine(environment.WebRootPath, "giaovien", hocSinh.TenLinkAnh);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                hocSinh.TenLinkAnh = newFileName;
            }



            context.SaveChanges();

            ViewData["Id"] = hocSinh.Id;
            ViewData["LopId"] = hocSinh.IdLopHoc;
            ViewData["TenLinkAnh"] = hocSinh.TenLinkAnh;
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var hocSinh = context.NguoiDungs.Find(id);
            if (hocSinh == null)
            {
                return RedirectToAction("Index", "HocSinh");
            }
            hocSinh.TrangThai = 0;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
