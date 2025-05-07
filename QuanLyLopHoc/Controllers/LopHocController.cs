using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyLopHoc.Controllers
{
    [Authorize]
    public class LopHocController : Controller
    {
        private readonly AppDbContext context;

        public LopHocController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            ViewBag.DanhSachLopHoc = context.LopHocs.ToList();
            return View(new LopHocDTO());
        }
     
        [HttpPost]
        public IActionResult Index(LopHocDTO lopHocDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DanhSachLopHoc = context.LopHocs.ToList();
                return View(lopHocDto);
            }

            var lop = new LopHoc
            {
                TenLop = lopHocDto.TenLop,
                KhoiLop = lopHocDto.KhoiLop,
                TrangThai = 1
            };
            context.LopHocs.Add(lop);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var lopHoc = context.LopHocs.Find(id);
            if (lopHoc == null)
            {
                return RedirectToAction("Index", "LopHoc");
            }
            ViewData["LopId"] = lopHoc.Id;
            var lopHocDto = new LopHocDTO
            {
                TenLop = lopHoc.TenLop,
                KhoiLop = lopHoc.KhoiLop,
                TrangThai = lopHoc.TrangThai
            };

            return View(lopHocDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, LopHocDTO lopHocDto)
        {
            var lopHocToUpdate = context.LopHocs.Find(id);
            if (lopHocToUpdate == null)
            {
                return RedirectToAction("Index", "LopHoc");
            }        
            ViewData["LopId"] = lopHocToUpdate.Id;
          
            lopHocToUpdate.TenLop = lopHocDto.TenLop;
            lopHocToUpdate.KhoiLop = lopHocDto.KhoiLop;
            lopHocToUpdate.TrangThai = lopHocDto.TrangThai;
            context.SaveChanges();
            return RedirectToAction("Index", "LopHoc");
        }

        public IActionResult Delete(int id)
        {
            var lopHoc = context.LopHocs.Find(id);
            if (lopHoc == null)
            {
                return RedirectToAction("Index", "LopHoc");
            }
            context.LopHocs.Remove(lopHoc);
            context.SaveChanges(true);
            return RedirectToAction("Index", "LopHoc");
        }
    }
}
