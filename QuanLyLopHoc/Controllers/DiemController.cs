using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class DiemController : Controller
    {
        private readonly AppDbContext context;

        public DiemController(AppDbContext context)
        {
            this.context = context;
                 
        }
        public IActionResult Index()
        {
            var danhSach = context.Diems
                .Where(x => x.TrangThai == 1)
                .GroupBy(x => x.TenDiem) 
                .Select(g => g.First()) 
                .Select(x => new DiemDto
                {
                    Id = x.Id,
                    TenDiem = x.TenDiem,
                    TrangThai = x.TrangThai
                })
                .ToList();

            ViewBag.DanhSachDiem = danhSach;
            return View();
        }

        [HttpPost]
        public IActionResult Index(DiemDto diemDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DanhSachDiem = context.Diems.ToList();
                return View(diemDto);
            }

            var diem = new Diem
            {
                TenDiem = diemDto.TenDiem,
                TrangThai = 1
            };

            context.Diems.Add(diem);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var diem = context.Diems.Find(id);
            if (diem == null)
            {
                return RedirectToAction("Index", "Diem");
            }
            ViewData["DiemId"] = diem.Id;
            var diemDto = new DiemDto
            {
                TenDiem = diem.TenDiem,
                TrangThai = diem.TrangThai
            };
            return View(diemDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, DiemDto diemDto)
        {
            var diem = context.Diems.Find(id);
            if (diem == null)
            {
                return RedirectToAction("Index", "Diem");
            }
            diem.TenDiem = diemDto.TenDiem;
            diem.TrangThai = diemDto.TrangThai;
            context.SaveChanges();
            return RedirectToAction("Index", "Diem");
        }

        public IActionResult Delete(int id)
        {
            var diem = context.Diems.Find(id);
            if (diem == null)
            {
                return RedirectToAction("Index", "Diem");
            }
            diem.TrangThai = 0;
            context.SaveChanges();
            return RedirectToAction("Index", "Diem");
        }

    }
}
