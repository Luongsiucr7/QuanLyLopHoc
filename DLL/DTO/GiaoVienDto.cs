using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class GiaoVienDto
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        public string TenNguoiDung { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải là địa chỉ Gmail hợp lệ.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string DiaChi { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^0[1-9]\d{8}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0, số thứ hai từ 1-9 và gồm 10 chữ số.")]
        public string SoDienThoai { get; set; } = null!;

        public bool GioiTinh { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DoTuoiHopLe(23, 60)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Vai trò là bắt buộc.")]
        public int VaiTro { get; set; }

        public string? TenLinkAnh { get; set; }


        [Required(ErrorMessage = "Chọn một trạng thái")]
        public int TrangThai { get; set; }



        [Required(ErrorMessage = "Chọn ít nhất 1 lớp dạy")]
        public List<int> DanhSachIdLop { get; set; } = new();

        [Required(ErrorMessage = "Chọn ít nhất 1 môn dạy")]
        public List<int> DanhSachIdMon { get; set; } = new();



    }
}
