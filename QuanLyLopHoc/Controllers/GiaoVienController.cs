using System.Linq;
using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GiaoVienController : Controller
    {

        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public GiaoVienController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        private void LoadData(int page = 1, int pageSize = 10)
        {
            var idGiaoVien = HttpContext.Session.GetInt32("Id");
            var giaoVienQuery = context.NguoiDungs
                .Where(x => x.VaiTro == 1 && x.TrangThai == 1)
                .ToList();

            var lopHoc = context.LopHocs
                .Where(x => x.TrangThai == 1)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.TenLop
                }).ToList();


            var danhSachMon = context.MonHocs
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.TenMon
                }).ToList();

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


            ViewBag.DanhSachMon = danhSachMon;
            ViewBag.DanhSachLop = lopHoc;
            ViewBag.MonTheoNguoiDungId = monTheoNguoiDung;
            ViewBag.DanhSachGiaoVien = pagedGiaoVien;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            LoadData(page, pageSize);

            return View(new GiaoVienDto());
        }


        [HttpPost]
        public IActionResult Index(GiaoVienDto giaoVienDto, IFormFile TenLinkAnh , int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                LoadData(page, pageSize);
                ViewBag.ShowModal = true;
                return View(giaoVienDto);

            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(TenLinkAnh.FileName);
            string path = Path.Combine(environment.WebRootPath, "giaovien", newFileName);
            using (var stream = new FileStream(path, FileMode.Create))
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

            foreach (var idLop in giaoVienDto.DanhSachIdLop)
            {
                foreach (var idMon in giaoVienDto.DanhSachIdMon)
                {
                    
                    var daTonTai = context.LopMons.Any(lm =>
                        lm.IdLop == idLop &&
                        lm.IdMonHoc == idMon &&
                        lm.IdNguoiDung == giaoVien.Id);

                    if (!daTonTai)
                    {
                        var lopMon = new LopMon
                        {
                            IdLop = idLop,
                            IdMonHoc = idMon,
                            IdNguoiDung = giaoVien.Id,
                            TrangThai = 1
                        };
                        context.LopMons.Add(lopMon);
                    }
                }
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var idGiaoVien = HttpContext.Session.GetInt32("Id");
            var giaoVien = context.NguoiDungs.Find(id);
            if (giaoVien == null)
            {
                return RedirectToAction("Index", "GiaoVien");
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
            giaoVien.TenNguoiDung = giaoVienDto.TenNguoiDung;
            giaoVien.Email = giaoVienDto.Email;
            giaoVien.SoDienThoai = giaoVienDto.SoDienThoai;
            giaoVien.DiaChi = giaoVienDto.DiaChi;
            giaoVien.GioiTinh = giaoVienDto.GioiTinh;
            giaoVien.NgaySinh = DateOnly.FromDateTime(giaoVienDto.NgaySinh);

            if (TenLinkAnh != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(TenLinkAnh.FileName);
                string path = Path.Combine(environment.WebRootPath, "giaovien", newFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    TenLinkAnh.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(giaoVien.TenLinkAnh))
                {
                    string oldPath = Path.Combine(environment.WebRootPath, "giaovien", giaoVien.TenLinkAnh);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                giaoVien.TenLinkAnh = newFileName;
            }

            context.SaveChanges();


            ViewData["Id"] = giaoVien.Id;
            ViewData["TenLinkAnh"] = giaoVien.TenLinkAnh;
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var giaoVien = context.NguoiDungs.Find(id);
            if (giaoVien == null)
            {
                return RedirectToAction("Index", "GiaoVien");
            }
            giaoVien.TrangThai = 0;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ThemChuNhiem()
        {
            var giaoVienChuaChuNhiem = context.NguoiDungs
               .Where(nd => nd.VaiTro == 1)
               .Where(nd => !context.LopGiaoViens.Any(lgv => lgv.IdNguoiDung == nd.Id && lgv.TrangThai == 1))
               .ToList();

            var lopChuaChuNhiem = context.LopHocs
                .Where(lh => !context.LopGiaoViens.Any(lgv => lgv.IdLopHoc == lh.Id && lgv.TrangThai == 1))
                .ToList();

            ViewBag.GiaoVienChuaChuNhiem = giaoVienChuaChuNhiem;
            ViewBag.LopChuaChuNhiem = lopChuaChuNhiem;

            return View();
        }

        [HttpPost]
        public IActionResult ThemChuNhiem(int id_nguoi_dung, int id_lop_hoc)
        {
            var chuNhiem = new LopGiaoVien
            {
                IdNguoiDung = id_nguoi_dung,
                IdLopHoc = id_lop_hoc,
                TrangThai = 1
            };

            context.LopGiaoViens.Add(chuNhiem);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
