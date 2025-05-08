using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class HocLucDto
    {
        public string TenHocSinh { get; set; } = null!;
        public decimal? DiemToan_GK { get; set; }
        public decimal? DiemVan_GK { get; set; }
        public decimal? DiemLichSu_GK { get; set; }
        public decimal? DiemNgoaiNgu_GK { get; set; }
        public decimal? DiemKhoaHoc_GK { get; set; }
        public decimal? DiemTheDuc_GK { get; set; }
        public decimal? DiemTinHoc_GK { get; set; }
        public decimal? DiemToan_CK { get; set; }
        public decimal? DiemVan_CK { get; set; }
        public decimal? DiemLichSu_CK { get; set; }
        public decimal? DiemNgoaiNgu_CK { get; set; }
        public decimal? DiemKhoaHoc_CK { get; set; }
        public decimal? DiemTheDuc_CK { get; set; }
        public decimal? DiemTinHoc_CK { get; set; }
        public decimal? DiemTrungBinh { get; set; }
        public string LopHoc { get; set; } = null!;
    }
}
