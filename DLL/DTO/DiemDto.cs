using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class DiemDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên điểm không được để trống!")]
        public string TenDiem { get; set; } = null!;
        [Required(ErrorMessage = "Chọn một trạng thái")]
        public int TrangThai { get; set; }
    }
}

