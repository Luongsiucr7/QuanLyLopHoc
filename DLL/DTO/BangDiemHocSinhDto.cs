using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class BangDiemHocSinhDto
    {
        public string TenHocSinh { get; set; } = null!;
        public decimal? DiemToan { get; set; }
        public decimal? DiemVan { get; set; }
        public decimal? DiemLichSu { get; set; }
        public decimal? DiemNgoaiNgu { get; set; }
        public decimal? DiemKhoaHoc { get; set; }
        public decimal? DiemTheDuc { get; set; }
        public decimal? DiemTinHoc { get; set; }
        public string LopHoc { get; set; } = null!;
        public string? GiaoVienChuNhiem { get; set; }
        public double? DiemTongKet { get; set; } 
        public string? HocLuc { get; set; }
    }
}
