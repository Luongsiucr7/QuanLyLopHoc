using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class MonHocDto
    {
        [Required(ErrorMessage = "Tên môn không được để trống!")]
        public string TenMon { get; set; } = null!;
        [Required(ErrorMessage ="Chọn một trạng thái")]
        public int TrangThai { get; set; }
    }
}
