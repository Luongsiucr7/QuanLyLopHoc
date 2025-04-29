using System;
using System.Collections.Generic;

namespace DLL.Model;

public partial class NguoiDung
{
    public int Id { get; set; }

    public string MaNguoiDung { get; set; } = null!;

    public string TenNguoiDung { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? NhanXet { get; set; }

    public bool GioiTinh { get; set; }

    public DateOnly NgaySinh { get; set; }

    public string MatKhau { get; set; } = null!;

    public int VaiTro { get; set; }

    public string? TenLinkAnh { get; set; }

    public int TrangThai { get; set; }

    public int? IdLopHoc { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual LopHoc? IdLopHocNavigation { get; set; }

    public virtual ICollection<LopGiaoVien> LopGiaoViens { get; set; } = new List<LopGiaoVien>();

    public virtual ICollection<LopMon> LopMons { get; set; } = new List<LopMon>();
}
