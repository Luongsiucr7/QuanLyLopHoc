using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyLopHoc.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly AppDbContext context;

        public TrangChuController(AppDbContext context)
        {
            this.context = context;
        }
        
        public IActionResult Index()
        {
            var lopHoc = context.LopHocs.ToList();
            return View(lopHoc);
        }



    }
}
