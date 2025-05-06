using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
    public class HocSinhDto
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        public string TenNguoiDung { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string DiaChi { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; } = null!;

        public bool GioiTinh { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Vai trò là bắt buộc.")]
        public int VaiTro { get; set; }

        public string? TenLinkAnh { get; set; }

        public int TrangThai { get; set; }

        public int? IdLopHoc { get; set; }
    }
    
}
