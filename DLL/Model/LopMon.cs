using System;
using System.Collections.Generic;

namespace DLL.Model;

public partial class LopMon
{
    public int Id { get; set; }

    public int? IdLop { get; set; }

    public int? IdMonHoc { get; set; }

    public int TrangThai { get; set; }

    public int? IdNguoiDung { get; set; }

    public virtual LopHoc? IdLopNavigation { get; set; }

    public virtual MonHoc? IdMonHocNavigation { get; set; }

    public virtual NguoiDung? IdNguoiDungNavigation { get; set; }
}
