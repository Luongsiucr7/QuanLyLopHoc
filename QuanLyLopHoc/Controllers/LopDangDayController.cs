﻿using DLL.DTO;
using DLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace QuanLyLopHoc.Controllers
{
    [Authorize(Roles = "GiaoVien")]
    public class LopDangDayController : Controller
    {
        private readonly AppDbContext context;

        public LopDangDayController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int? idLopHoc)
        {
            var idGiaovien = HttpContext.Session.GetInt32("Id");
            if (idGiaovien == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách lớp mà giáo viên đang dạy
            var lopDangDayList = (from lm in context.LopMons
                                  join lh in context.LopHocs on lm.IdLop equals lh.Id
                                  where lm.IdNguoiDung == idGiaovien && lm.TrangThai == 1
                                  select new SelectListItem
                                  {
                                      Value = lh.Id.ToString(),
                                      Text = lh.TenLop
                                  }).Distinct().ToList();

            ViewBag.LopDangDay = lopDangDayList;

            // Nếu không chọn lớp, không thực hiện truy vấn bảng điểm
            if (idLopHoc == null)
            {
                return View();
            }


            var data = (from lm in context.LopMons
                        join mh in context.MonHocs on lm.IdMonHoc equals mh.Id
                        join lh in context.LopHocs on lm.IdLop equals lh.Id
                        join nd in context.NguoiDungs on lh.Id equals nd.IdLopHoc
                        where lm.IdLop == idLopHoc
                           && lm.IdNguoiDung == idGiaovien
                           && lm.TrangThai == 1
                           && nd.VaiTro == 0
                           && nd.TrangThai == 1
                        join d in context.Diems
                            on new { ndId = nd.Id, mhId = mh.Id }
                            equals new { ndId = d.IdNguoiDung.Value, mhId = d.IdMonHoc.Value } into diemJoin
                        from d in diemJoin.DefaultIfEmpty()
                        group d by new { nd.Id, nd.TenNguoiDung, mh.TenMon,lh.TenLop } into g
                        select new LopDangDayDto
                        {
                            IdHocSinh = g.Key.Id,
                            IdMon = context.MonHocs.FirstOrDefault(m => m.TenMon == g.Key.TenMon).Id,
                            TenHocSinh = g.Key.TenNguoiDung,
                            TenMon = g.Key.TenMon,
                            TenLop = g.Key.TenLop,
                            DiemGiuaKy = g.Where(x => x != null && x.TenDiem == "Giữa Kỳ").Select(x => x.SoDiem).FirstOrDefault(),
                            DiemCuoiKy = g.Where(x => x != null && x.TenDiem == "Cuối Kỳ").Select(x => x.SoDiem).FirstOrDefault()
                        }).ToList();

            return View(data);
        }

        [HttpPost]
        public IActionResult Index(LopDangDayDto lop)
        {
            var idGiaoVien = HttpContext.Session.GetInt32("Id");

            var diemGiuaKy = context.Diems
                .FirstOrDefault(d => d.IdNguoiDung == lop.IdHocSinh && d.IdMonHoc == lop.IdMon && d.TenDiem == "Giữa Kỳ");

            var diemCuoiKy = context.Diems
                .FirstOrDefault(d => d.IdNguoiDung == lop.IdHocSinh && d.IdMonHoc == lop.IdMon && d.TenDiem == "Cuối Kỳ");

            if (diemGiuaKy != null)
            {
                diemGiuaKy.SoDiem = lop.DiemGiuaKy;
            }
            else
            {
                context.Diems.Add(new DLL.Model.Diem
                {
                    IdNguoiDung = lop.IdHocSinh,
                    IdMonHoc = lop.IdMon,
                    TenDiem = "Giữa Kỳ",
                    SoDiem = lop.DiemGiuaKy
                });
            }

            if (diemCuoiKy != null)
            {
                diemCuoiKy.SoDiem = lop.DiemCuoiKy;
            }
            else
            {
                context.Diems.Add(new DLL.Model.Diem
                {
                    IdNguoiDung = lop.IdHocSinh,
                    IdMonHoc = lop.IdMon,
                    TenDiem = "Cuối Kỳ",
                    SoDiem = lop.DiemCuoiKy
                });
            }

            context.SaveChanges();

            return RedirectToAction("Index", new
            {
                 idLopHoc = context.LopMons
                .Where(lm => lm.IdNguoiDung == HttpContext.Session.GetInt32("Id") && lm.IdMonHoc == lop.IdMon)
                .Select(lm => lm.IdLop).FirstOrDefault()
            });
        }
    }
}
