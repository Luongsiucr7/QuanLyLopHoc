using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Model;

namespace DLL.DataSeed
{
    public class SeedDataAdmin
    {
        public static void SeedAdmins(AppDbContext context)
        {
            try
            {
                if (!context.NguoiDungs.Any(u => u.VaiTro == 2))
                {
                    var admin = new NguoiDung
                    { 

                        TenNguoiDung = "Nguyễn Đình Đức Lương",
                        MatKhau = "123456",
                        VaiTro = 2,
                        Email = "luongnddph50650@gmail.com",
                        DiaChi = "123 Đường Admin, Quận 1, TP.HCM",
                        SoDienThoai = "0345678912",
                        NhanXet = null,
                        GioiTinh = true,
                        NgaySinh = DateOnly.FromDateTime(new DateTime(1980, 1, 1)),
                        TenLinkAnh = null,
                        TrangThai = 1,
                        IdLopHoc = null
                    };

                    context.NguoiDungs.Add(admin);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework like Serilog or NLog)
                Console.WriteLine($"Error seeding admin: {ex.Message}");
                throw;
            }
        }
    }
}
