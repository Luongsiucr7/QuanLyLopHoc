using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class MonHocController : Controller
    {
        private readonly AppDbContext context;

        public MonHocController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            ViewBag.DanhSachMonHoc = context.MonHocs.Where(x => x.TrangThai ==1).ToList();
            return View(new MonHocDto());
        }

        [HttpPost]
        public IActionResult Index(MonHocDto monHocDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DanhSachMonHoc = context.MonHocs.ToList();
                return View(monHocDto);
            }

            var monHoc = new MonHoc
            {
                TenMon = monHocDto.TenMon,
                TrangThai = 1
            };

            context.MonHocs.Add(monHoc);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var monHoc = context.MonHocs.Find(id);
            if (monHoc == null)
            {
                return RedirectToAction("Index", "MonHoc");
            }
            ViewData["MonId"] = monHoc.Id;
            var monHocDto = new MonHocDto
            {
                TenMon = monHoc.TenMon,
                TrangThai = monHoc.TrangThai
            };
            return View(monHocDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, MonHocDto monHocDto)
        {
            var monHocToUpdate = context.MonHocs.Find(id);
            if (monHocToUpdate == null)
            {
                return RedirectToAction("Index", "MonHoc");
            }
            monHocToUpdate.TenMon = monHocDto.TenMon;
            monHocToUpdate.TrangThai = monHocDto.TrangThai;
            context.SaveChanges();
            return RedirectToAction("Index", "MonHoc");
        }

        public IActionResult Delete(int id)
        {
            var monHoc = context.MonHocs.Find(id);
            if (monHoc == null)
            {
                return RedirectToAction("Index", "MonHoc");
            }
            monHoc.TrangThai = 0;
            context.SaveChanges();
            return RedirectToAction("Index", "MonHoc");
        }
        }
}
