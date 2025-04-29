using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyLopHoc.Controllers
{
    public class LopHocController : Controller
    {
        private readonly AppDbContext context;

        public LopHocController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var lopHoc = context.LopHocs.ToList();
            return View(lopHoc);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(LopHocDTO lopHocDto)
        {
            LopHoc lop = new LopHoc();
            {
                lop.TenLop = lopHocDto.TenLop;
                lop.KhoiLop = lopHocDto.KhoiLop;
                lop.TrangThai = 1;
            }
            context.LopHocs.Add(lop);
            context.SaveChanges();

            return RedirectToAction("Index", "LopHoc");
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
