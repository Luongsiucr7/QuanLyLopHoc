using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class LopDangDayDto
    {
        public int IdHocSinh { get; set; }     

        public int IdMon { get; set; }

        public string TenHocSinh { get; set; }
        public string TenMon { get; set; }
        public string TenLop { get; set; }
        public decimal DiemGiuaKy { get; set; }
        public decimal DiemCuoiKy { get; set; }
    }
}
