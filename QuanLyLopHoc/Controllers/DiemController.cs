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
            ViewBag.DanhSachDiem = context.Diems
                 .GroupBy(d => d.TenDiem)
                 .Select(g => g.First())
                 .ToList();
            return View(new DiemDto());
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


    }
}
