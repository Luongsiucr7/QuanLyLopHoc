using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class LopHocDTO
    {
        [Required(ErrorMessage = "Tên lớp không được để trống!")]
        public string TenLop { get; set; } = null!;
        [Required(ErrorMessage = "Khối lớp không được để trống!")]
        public int KhoiLop { get; set; }
        public int TrangThai { get; set; }
    }
}
