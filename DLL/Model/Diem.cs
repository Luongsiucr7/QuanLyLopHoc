using System;
using System.Collections.Generic;

namespace DLL.Model;

public partial class Diem
{
    public int Id { get; set; }

    public string MaDiem { get; set; } = null!;

    public string TenDiem { get; set; } = null!;

    public decimal SoDiem { get; set; }

    public int? IdMonHoc { get; set; }

    public int? IdNguoiDung { get; set; }

    public int TrangThai { get; set; }

    public virtual MonHoc? IdMonHocNavigation { get; set; }

    public virtual NguoiDung? IdNguoiDungNavigation { get; set; }
}
