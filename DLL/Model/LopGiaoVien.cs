using System;
using System.Collections.Generic;

namespace DLL.Model;

public partial class LopGiaoVien
{
    public int Id { get; set; }

    public int? IdNguoiDung { get; set; }

    public int? IdLopHoc { get; set; }

    public int TrangThai { get; set; }

    public virtual LopHoc? IdLopHocNavigation { get; set; }

    public virtual NguoiDung? IdNguoiDungNavigation { get; set; }
}
